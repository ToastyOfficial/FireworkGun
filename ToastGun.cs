using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using System.Linq;
using UnityEngine;
using Oxide.Core.Libraries.Covalence;

namespace Oxide.Plugins
{
    [Info("Firework Gun", "Toasty", "2.0")]
    [Description("Shoot Firework")]
    class FireworkGun : RustPlugin
    {
        #region Fields
		private const ulong WEAPON_SKIN_ID = 889710179;
        private const int WEAPON_ID = 1545779598;

        #endregion

        #region Oxide Hooks
		private List<string> fpreflist = new List<string>()
        {
            "assets/prefabs/deployable/fireworks/mortarchampagne.prefab",
            "assets/prefabs/deployable/fireworks/mortargreen.prefab",
            "assets/prefabs/deployable/fireworks/mortarblue.prefab",
            "assets/prefabs/deployable/fireworks/mortarviolet.prefab",
            "assets/prefabs/deployable/fireworks/mortarred.prefab",
            "assets/prefabs/deployable/fireworks/romancandle-green.prefab"
        };
		

        private void OnPlayerInput(BasePlayer player, InputState input)
        {
            if (player == null || input == null)
                return;

            if (input.IsDown(BUTTON.FIRE_PRIMARY))
		{
				var selfpref = fpreflist[new System.Random().Next(fpreflist.Count)];
                Item activeItem = player.GetActiveItem();
                if (activeItem == null || activeItem.info.itemid != WEAPON_ID || activeItem.skin != WEAPON_SKIN_ID)
                    return;
                RepeatingFirework baseEntity = GameManager.server.CreateEntity(selfpref, player.eyes.position) as RepeatingFirework;

                baseEntity.enableSaving = false;
                baseEntity.transform.up = player.eyes.HeadForward();
                baseEntity.Spawn();

                baseEntity.ClientRPC(null, "RPCFire");

                baseEntity.Kill();
        }
		}
		
	
		        #endregion

        #region Commands
        [ChatCommand("toastgun")]
        private void cmdFirework(BasePlayer player, string command, string[] args)
        {
			if (!player.IsAdmin)
			{
				Puts("No permission to execute this command. You need auth level 2");
				return;
			}
            Item item = ItemManager.CreateByItemID(WEAPON_ID, 1, WEAPON_SKIN_ID);
            BaseProjectile baseProjectile = item.GetHeldEntity()?.GetComponent<BaseProjectile>();
            if (baseProjectile != null && baseProjectile.primaryMagazine.contents > 0)
                baseProjectile.primaryMagazine.contents = 0;

            player.GiveItem(item, BaseEntity.GiveItemReason.PickedUp);
           
        }
        #endregion
    }
}

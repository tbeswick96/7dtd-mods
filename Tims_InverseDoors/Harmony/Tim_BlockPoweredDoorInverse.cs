using System;
using System.Collections.Generic;
using System.Reflection;
using DMT;
using Harmony;
using UnityEngine;

public class Tim_BlockPoweredDoorInverse {
    public class Tim_BlockPoweredDoorInverse_Init : IHarmony {
        public void Start() {
            Debug.Log(" Loading Patch: " + this.GetType().ToString());
            var harmony = HarmonyInstance.Create(GetType().ToString());
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    [HarmonyPatch(typeof(BlockPoweredDoor))]
    [HarmonyPatch("updateAnimState")]
    [HarmonyPatch(new Type[] { typeof(BlockEntityData), typeof(bool), typeof(BlockValue) })]
    public class Tim_BlockPoweredDoorInverse_updateAnimState {
        public static bool Prefix(BlockPoweredDoor __instance, BlockEntityData _ebcd, bool _bOpen, BlockValue _blockValue) {
            if (_ebcd == null || !_ebcd.bHasTransform) {
                return false;
            }

            Animator[] componentsInChildren = _ebcd.transform.GetComponentsInChildren<Animator>();
            if (componentsInChildren == null) {
                return false;
            }

            // If this is an inverse variant door, flip open state
            if (__instance is BlockPoweredDoorInverse) {
                _bOpen = !_bOpen;
            }

            for (int index = componentsInChildren.Length - 1; index >= 0; --index) {
                Animator animator = componentsInChildren[index];
                animator.enabled = true;
                animator.SetBool("IsOpen", _bOpen);
                animator.SetTrigger("OpenTrigger");
            }
            return false;
        }
    }
}

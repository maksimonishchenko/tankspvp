﻿using UnityEngine;
using Morpeh.Globals;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Globals/Custom/" + nameof(JoinRoomFailedEvent))]
public sealed class JoinRoomFailedEvent : BaseGlobalEvent<(short,string)> {
}
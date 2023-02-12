﻿using UnityEngine.Events;

public class GlobalEventManager
{
    public static readonly UnityEvent<Target> OnTargetSpawned = new();
    public static readonly UnityEvent<Target> OnTargetHitCart = new();
    
    public static readonly UnityEvent<Projectile> OnProjectileSpawned = new();
    public static readonly UnityEvent<Projectile> OnProjectileThrown = new();
}
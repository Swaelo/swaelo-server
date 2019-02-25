﻿// ================================================================================================================================
// File:        BaseEntity.cs
// Description: All types of entities must implement this base class so they can be handled by the entity manager, among other things
// Author:      Harley Laurie          
// Notes:       
// ================================================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swaelo_Server
{
    public abstract class BaseEntity
    {
        public string ID = "-1";
        public string Type = "NULL";
        public Vector3 Position = Vector3.Zero;
        public Vector3 Scale = Vector3.Zero;
        public Quaternion Rotation = Quaternion.Identity;
        public Entity entity;
        public abstract void Update(float dt);
        public int HealthPoints = 3;
    }
}
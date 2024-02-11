// CTCL's Extras for People Playground - CTCL 2023-2024
// File: script.cs
// Purpose: Main mod functions
// Created: February 13, 2023
// Modified: February 11, 2024

using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

using Liquids;

namespace Mod
{
    public class Mod
    {
        public static void Main()
        {
            ModAPI.RegisterLiquid(BloodRemoverSyringe.BloodRemover.ID, new BloodRemoverSyringe.BloodRemover());
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Acid Syringe"),
                    NameOverride = "Blood Remover Syringe - CE",
                    DescriptionOverride = "Some sort of serum that removes blood from limbs.",
                    CategoryOverride = ModAPI.FindCategory("Chemistry"),
                    //ThumbnailOverride = ModAPI.LoadSprite("housing.png"),
                    AfterSpawn = (Instance) =>
                    {
                        UnityEngine.Object.Destroy(Instance.GetComponent<SyringeBehaviour>());
                        Instance.GetOrAddComponent<BloodRemoverSyringe>();
                    }
                }
            );
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Acid Syringe"),
                    NameOverride = "Blood Syringe - CE",
                    DescriptionOverride = "Syringe with blood since the game does not have one already.",
                    CategoryOverride = ModAPI.FindCategory("Chemistry"),
                    //ThumbnailOverride = ModAPI.LoadSprite("housing.png"),
                    AfterSpawn = (Instance) =>
                    {
                        UnityEngine.Object.Destroy(Instance.GetComponent<SyringeBehaviour>());
                        var bs = Instance.GetOrAddComponent<BloodSyringe>();
                        bs.TransferRate = 100f; // 1000x more
                    }
                }
            );
            // Activation Resizable Housing
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Resizable Housing"),
                    NameOverride = "Activation Resizable Housing - CE",
                    DescriptionOverride = "Resizable Housing that scales from activation. Red to increase size, blue to decrease size.",
                    CategoryOverride = ModAPI.FindCategory("Misc."),
                    ThumbnailOverride = ModAPI.LoadSprite("housing.png"),
                    AfterSpawn = (Instance) =>
                    {
                        Instance.AddComponent<ActResizeBehaviour>();
                        Instance.GetComponent<PhysicalBehaviour>().Resizable = false;
                    }
                }
            );
            // Short Cannon
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Detached 120mm Cannon"),
                    NameOverride = "Short Cannon - CE",
                    DescriptionOverride = "Detached 120mm Cannon but much shorter",
                    CategoryOverride = ModAPI.FindCategory("Firearms"),
                    ThumbnailOverride = ModAPI.LoadSprite("thumb.png"),
                    AfterSpawn = (Instance) =>
                    {
                        Vector2 newsize = new Vector2(0.85f, 0.35f);
                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("sprites/sp_shortcannon.png");
                        Instance.GetComponent<BoxCollider2D>().size = newsize;
                        Instance.GetComponent<MachineGunBehaviour>().barrelPosition = new Vector2(newsize.x / 2, 0);
                        Instance.GetComponent<Rigidbody2D>().mass = 1f;
                        //ca.Damage = 20.0f;
                        //ca.Recoil = 4.0f;
                        //ca.ImpactForce = 0.55f;
                        //ca.StartSpeed = 200f;
                        //ca.PenetrationRandomAngleMultiplier = 0.0001f;
                        //Instance.GetComponent<FirearmBehaviour>().Cartridge = ca;
                    }
                }
            );
            int[] flasksizes = {2, 3, 4, 5, 10, 20, 50, 100};
            foreach (int size in flasksizes) {
                ModAPI.Register(
                    new Modification()
                    {
                        OriginalItem = ModAPI.FindSpawnable("Flask"),
                        NameOverride = String.Concat(size, "x Capacity Flask - CE"),
                        DescriptionOverride = String.Concat(size, "x capacity flask"),
                        CategoryOverride = ModAPI.FindCategory("Chemistry"),
                        //ThumbnailOverride = ModAPI.LoadSprite("thumb.png"),
                        AfterSpawn = (Instance) =>
                        {
                            Instance.GetComponent<FlaskBehaviour>().Capacity = Liquid.LiterToLiquidUnit * (float)size;
                        }
                    }
                );
            }
            // Air Brake
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Brick"),
                    NameOverride = "Air Brake - CE",
                    DescriptionOverride = "Slows down mid-air when activated, becomes more effective when powered",
                    CategoryOverride = ModAPI.FindCategory("Machinery"),
                    // TODO: Make custom sprite and adjust object sizes accordingly
                    //ThumbnailOverride = ModAPI.LoadSprite("thumb.png"),
                    AfterSpawn = (Instance) =>
                    {
                        //Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("sprites/sp_airbrake.png");
                        Instance.AddComponent<AirBrakeBehaviour>();
                    }
                }
            );
            // Cartridge
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Small I-Beam"),
                    NameOverride = "Cartridge - CE",
                    DescriptionOverride = "156mm Cartridge, good for making blowback-operated cannons",
                    CategoryOverride = ModAPI.FindCategory("Explosives"),
                    ThumbnailOverride = ModAPI.LoadSprite("sprites/sp_cartridge.png"),
                    AfterSpawn = (Instance) =>
                    {
                        Sprite[] spriteArray = new Sprite[] {
                            ModAPI.LoadSprite("sprites/sp_cartridge.png"),
                            ModAPI.LoadSprite("sprites/sp_cartridge_empty.png"),
                            ModAPI.LoadSprite("sprites/sp_bullet.png"),
                        };
                        
                        AudioClip[] clips = new AudioClip[] {
                            ModAPI.LoadSound("sfx/very-small-explosion-01.wav"),
                            ModAPI.LoadSound("sfx/very-small-explosion-02.wav"),
                            ModAPI.LoadSound("sfx/very-small-explosion-03.wav"),
                            ModAPI.LoadSound("sfx/very-small-explosion-04.wav")
                        };
                        
                        Instance.GetComponent<SpriteRenderer>().sprite = spriteArray[0];
                        Instance.FixColliders();
                        Instance.GetComponent<PolygonCollider2D>().offset = new Vector2(0f, 0f);

                        var cb = Instance.AddComponent<CartridgeBehaviour>();
                        cb.sprites = spriteArray;
                        cb.clips = clips;
                    }
                }
            );
            // iPod Touch
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Brick"),
                    NameOverride = "iPod Touch - CE",
                    DescriptionOverride = "Model A2178",
                    CategoryOverride = ModAPI.FindCategory("Misc."),
                    ThumbnailOverride = ModAPI.LoadSprite("sprites/sp_ipod.png"),
                    AfterSpawn = (Instance) =>
                    {
                        Vector2 newsize = new Vector2(ModAPI.PixelSize * 15, ModAPI.PixelSize * 24);
                        Instance.GetComponent<BoxCollider2D>().size = newsize;

                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("sprites/sp_ipod.png");

                        Instance.GetComponent<BoxCollider2D>().size = newsize;

                        AudioSource ias = Instance.AddComponent<AudioSource>();
                        AudioClip iclip = ModAPI.LoadSound("sfx/tritone_hpf.wav");
                        
                        ias.clip = iclip;
                        
                        // Impact Force Threshold
                        float imp_force = 5f;
                        // Dispatch Chance
                        float dis_chance = 1.0f;

                        Instance.AddComponent<ActOnCollide>();
                        Instance.GetComponent<ActOnCollide>().ImpactForceThreshold = imp_force;
                        Instance.GetComponent<ActOnCollide>().DispatchChance = dis_chance;

                        void Ping() {
                            ias.Play();
                        }

                        UnityEvent CollideEvent;

                        CollideEvent = new UnityEvent();
                        CollideEvent.AddListener(Ping);

                        Instance.GetComponent<ActOnCollide>().Actions = CollideEvent;
                        
                    }
                }
            );
            // Spark Plug
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Flask"),
                    NameOverride = "Spark Plug - CE",
                    DescriptionOverride = "Creats a small explosion that scales with the amount of Oil stored. All Oil is used on activation.",
                    CategoryOverride = ModAPI.FindCategory("Machinery"),
                    ThumbnailOverride = ModAPI.LoadSprite("sprites/sp_sparkplug.png"),
                    AfterSpawn = (Instance) =>
                    {
                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("sprites/sp_sparkplug.png");

                        Instance.GetComponent<PhysicalBehaviour>().Properties = ModAPI.FindPhysicalProperties("Hollow metal");

                        Vector2 newsize = new Vector2(ModAPI.PixelSize * 24, ModAPI.PixelSize * 16);
                        Instance.GetComponent<BoxCollider2D>().size = newsize;

                        Instance.GetComponent<PhysicalBehaviour>().ActivationPropagationDelay = 0f;

                        AudioClip[] clips = new AudioClip[] {
                            ModAPI.LoadSound("sfx/very-small-explosion-01.wav"),
                            ModAPI.LoadSound("sfx/very-small-explosion-02.wav"),
                            ModAPI.LoadSound("sfx/very-small-explosion-03.wav"),
                            ModAPI.LoadSound("sfx/very-small-explosion-04.wav")
                        };

                        SparkPlugBehaviour sp = Instance.GetOrAddComponent<SparkPlugBehaviour>();
                        // Assign AudioClip objects
                        sp.clips = clips;
                    }
                }
            );
            // Bullet Box
            ModAPI.Register(
                new Modification() {
                    OriginalItem = ModAPI.FindSpawnable("9mm Pistol"),
                    NameOverride = "Bullet Box - CE",
                    DescriptionOverride = "Box that bullets come out of. Could be used for engines.",
                    CategoryOverride = ModAPI.FindCategory("Firearms"),
                    ThumbnailOverride = ModAPI.LoadSprite("sprites/sp_bulletbox.png"),
                    AfterSpawn = (Instance) => {
                        float baseforce = 0.5f;

                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("sprites/sp_bulletbox.png");
                        Instance.GetComponent<PhysicalBehaviour>().Properties = ModAPI.FindPhysicalProperties("Hollow metal");
                        Instance.FixColliders();

                        Instance.GetComponent<PhysicalBehaviour>().ActivationPropagationDelay = 0f;

                        Cartridge ca = ModAPI.FindCartridge("50 BMG");

                        Instance.GetComponent<FirearmBehaviour>().barrelPosition = new Vector2(ModAPI.PixelSize * 4.5f, 0f);
                        Instance.GetComponent<FirearmBehaviour>().barrelDirection = new Vector2(1f, 0f);
                        Instance.GetComponent<FirearmBehaviour>().EjectShells = false;
                        Instance.GetComponent<FirearmBehaviour>().MuzzleShockwave = false;
                        Instance.GetComponent<FirearmBehaviour>().InitialInaccuracy = 0.001f;
                        Instance.GetComponent<FirearmBehaviour>().BulletsPerShot = 1;


                        ca.Damage = 800.0f;
                        ca.Recoil = 4.0f;
                        ca.ImpactForce = 2f;
                        ca.StartSpeed = 1000f;
                        ca.PenetrationRandomAngleMultiplier = 0.0001f;
                        Instance.GetComponent<FirearmBehaviour>().Cartridge = ca;
                    }
                }
            );
            // Instant Liquid Valve
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Liquid Valve"),
                    NameOverride = "Instant Liquid Valve - CE",
                    DescriptionOverride = "Liquid Valve that has an instant turn duration",
                    CategoryOverride = ModAPI.FindCategory("Machinery"),
                    AfterSpawn = (Instance) =>
                    {
                        Instance.GetComponent<ValveBehaviour>().TurnDuration = 0.00001f;
                        Instance.GetComponent<PhysicalBehaviour>().ActivationPropagationDelay = 0f;
                    }
                }
            );
            // Instant Metal Detector
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Metal Detector"),
                    NameOverride = "Instant Metal Detector - CE",
                    DescriptionOverride = "Instant Metal Detector",
                    CategoryOverride = ModAPI.FindCategory("Machinery"),
                    //ThumbnailOverride = ModAPI.LoadSprite("sprites/sp_smallmagnet.png"),
                    AfterSpawn = (Instance) =>
                    {
                        Instance.GetComponent<PhysicalBehaviour>().ActivationPropagationDelay = 0f;
                        Instance.GetComponent<Activations.NodeBehaviour>().DelaySeconds = 0f;
                    }
                }
            );
            // Instant Gate
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Gate"),
                    NameOverride = "Instant Gate - CE",
                    DescriptionOverride = "Instant Gate",
                    CategoryOverride = ModAPI.FindCategory("Machinery"),
                    AfterSpawn = (Instance) =>
                    {
                        Instance.GetComponent<PhysicalBehaviour>().ActivationPropagationDelay = 0f;
                        Instance.GetComponent<Activations.NodeBehaviour>().DelaySeconds = 0f;
                    }
                }
            );
            // Variable Battery
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Battery"),
                    NameOverride = "Variable Battery - CE",
                    DescriptionOverride = "Battery that changes power level on activation. Red to increase, blue to decrease.",
                    CategoryOverride = ModAPI.FindCategory("Machinery"),
                    AfterSpawn = (Instance) =>
                    {
                        Instance.GetComponent<PhysicalBehaviour>().ActivationPropagationDelay = 0f;
                        Instance.AddComponent<VariableBatteryBehaviour>();
                        // TODO: Fix broken meter thing
                    }
                }
            );
            // High Density Metal Wheel
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Metal Wheel"),
                    NameOverride = "High Density Metal Wheel - CE",
                    DescriptionOverride = "Metal wheel that has ten times the mass",
                    CategoryOverride = ModAPI.FindCategory("Misc."),
                    ThumbnailOverride = ModAPI.LoadSprite("sprites/sp_highdensitywheel.png"),
                    AfterSpawn = (Instance) =>
                    {
                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("sprites/sp_highdensitywheel.png");
                        ModResources.SetMass(Instance, 30f);
                        //Instance.GetComponent<PhysicalBehaviour>().InitialMass = InitialMass * 10f;
                    }
                }
            );
            // EHDW
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Metal Wheel"),
                    NameOverride = "Extremely High Density Wheel - CE",
                    DescriptionOverride = "Metal wheel made of an extremely dense material of some sort. Weighs 100,000 kg.",
                    CategoryOverride = ModAPI.FindCategory("Machinery"),
                    ThumbnailOverride = ModAPI.LoadSprite("sprites/sp_ehdw.png"),
                    AfterSpawn = (Instance) =>
                    {
                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("sprites/sp_ehdw.png");
                        ModResources.SetMass(Instance, 2500f);
                    }
                }
            );
            // Generator Wheel
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Metal Wheel"),
                    NameOverride = "Generator Wheel - CE",
                    DescriptionOverride = "Generator Wheel",
                    CategoryOverride = ModAPI.FindCategory("Misc."),
                    AfterSpawn = (Instance) =>
                    {
                        Instance.AddComponent<AngularVelocityChargeBehaviour>();
                    }
                }
            );
            // Activation Meter
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Thermometer"),
                    NameOverride = "Activation Meter - CE",
                    DescriptionOverride = "Shows how many activations per second.",
                    CategoryOverride = ModAPI.FindCategory("Machinery"),
                    AfterSpawn = (Instance) =>
                    {
                        ActivationMeterBehaviour behaviour = Instance.GetOrAddComponent<ActivationMeterBehaviour>();
                        behaviour.tm = Instance.GetComponent<ThermometerBehaviour>().TextMesh;
                        GameObject.Destroy(Instance.GetComponent<ThermometerBehaviour>());
                    }
                }
            );
            // Electromagnet
            //ModAPI.Register(
            //    new Modification()
             //   {
            //        OriginalItem = ModAPI.FindSpawnable("Rod"),
            //        NameOverride = "Electromagnet - CE",
            //        DescriptionOverride = "Magnet that pulls metal objects towards it with electricity",
            //        CategoryOverride = ModAPI.FindCategory("Machinery"),
            //        AfterSpawn = (Instance) =>
            //        {
            //            Instance.AddComponent<ElectromagnetBehaviour>();
            //        
            //        }
            //    }
            //);        
            // High Friction Metal Wheel
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Metal Wheel"),
                    NameOverride = "High Friction Metal Wheel - CE",
                    DescriptionOverride = "Metal wheel that probably has more friction",
                    CategoryOverride = ModAPI.FindCategory("Misc."),
                    AfterSpawn = (Instance) =>
                    {
                        Instance.GetComponent<PhysicsMaterial2D>().friction = 100;
                    }
                }
            );
            // CVT wheel
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Piston"),
                    NameOverride = "CVT Wheel - CE",
                    DescriptionOverride = "Two wheels connected to each other, one of the wheels get bigger with angular velocity.",
                    CategoryOverride = ModAPI.FindCategory("Machinery"),
                    ThumbnailOverride = ModAPI.LoadSprite("sprites/sp_wheel.png"),
                    AfterSpawn = (Instance) =>
                    {
                        GameObject.Destroy(Instance.GetComponent<PistonBehaviour>());
                        GameObject.Destroy(Instance.GetComponent<SliderJoint2D>());
                        GameObject.Destroy(Instance.GetComponent<DamagableMachineryBehaviour>());
                        GameObject.Destroy(Instance.GetComponent<MuteBehaviour>());
                        
                        foreach(Collider2D col in Instance.GetComponents<Collider2D>()) GameObject.Destroy(col);
                        Instance.AddComponent<CircleCollider2D>();
                        // For some reason the collider is not the same size as the sprite on spawn
                        Instance.GetComponent<CircleCollider2D>().radius = 0.4857143f;
                        Instance.transform.localScale = new Vector2(1, 1);
                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("sprites/sp_wheel.png");
                        Instance.GetComponent<PhysicalBehaviour>().RefreshOutline();
                        Instance.GetComponent<PhysicalBehaviour>().CalculateCircumference();
                        Instance.GetComponent<PhysicalBehaviour>().BakeColliderGridPoints();
                        
                        // Use the existing child object
                        GameObject cvtwheel = Instance.transform.Find("PistonHead").gameObject;
                        cvtwheel.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("sprites/sp_wheel.png");
                        foreach(Collider2D col in cvtwheel.GetComponents<Collider2D>()) GameObject.Destroy(col);
                        cvtwheel.AddComponent<CircleCollider2D>();
                        cvtwheel.transform.SetParent(Instance.transform);
                        cvtwheel.transform.localScale = new Vector2(1, 1);
                        cvtwheel.GetOrAddComponent<CVTWheelBehaviour>().parentobj = Instance;
                        cvtwheel.GetComponent<PhysicalBehaviour>().RefreshOutline();
                        cvtwheel.GetComponent<PhysicalBehaviour>().CalculateCircumference();
                        cvtwheel.GetComponent<PhysicalBehaviour>().BakeColliderGridPoints();
                        Vector3 scale = Instance.transform.localScale;
                        cvtwheel.transform.position = new Vector3(Instance.transform.position.x, Instance.transform.position.y, Instance.transform.position.z);
                        FixedJoint2D fjoint = cvtwheel.GetOrAddComponent<FixedJoint2D>();
                        fjoint.connectedBody = Instance.GetComponent<Rigidbody2D>();
                        
                        //GameObject.Destroy(cvtwheel.GetComponent<Optout>());
                    }
                }
            );
            // Centrifugal Clutch
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Metal Wheel"),
                    NameOverride = "Centrifugal Clutch - CE",
                    DescriptionOverride = "Wheels that engage at a certain rotational velocity.",
                    CategoryOverride = ModAPI.FindCategory("Machinery"),
                    //ThumbnailOverride = ModAPI.LoadSprite(""),
                    AfterSpawn = (Instance) =>
                    {
                        GameObject cvtwheel = ModAPI.CreatePhysicalObject("cvtwheelthing", ModAPI.LoadSprite("sprites/sp_wheel.png"));
                        foreach(Collider2D col in cvtwheel.GetComponents<Collider2D>()) GameObject.Destroy(col);
                        cvtwheel.AddComponent<CircleCollider2D>();
                        cvtwheel.transform.SetParent(Instance.transform);
                        cvtwheel.GetComponent<PhysicalBehaviour>().RefreshOutline();
                        cvtwheel.GetComponent<PhysicalBehaviour>().CalculateCircumference();
                        cvtwheel.GetComponent<PhysicalBehaviour>().BakeColliderGridPoints();
                        // The driven is slightly smaller to be able to differeniate between the two
                        cvtwheel.transform.localScale = new Vector2(0.9f, 0.9f);
                        cvtwheel.AddComponent<CentrifugalClutchBehaviour>();
                        cvtwheel.GetComponent<CentrifugalClutchBehaviour>().parentobj = Instance;
                        Vector3 scale = Instance.transform.localScale;
                        cvtwheel.transform.position = new Vector3(Instance.transform.position.x, Instance.transform.position.y, Instance.transform.position.z);
                        FixedJoint2D fjoint = cvtwheel.GetOrAddComponent<FixedJoint2D>();
                        fjoint.connectedBody = Instance.GetComponent<Rigidbody2D>();
                        HingeJoint2D hjoint = cvtwheel.GetOrAddComponent<HingeJoint2D>();
                        hjoint.connectedBody = Instance.GetComponent<Rigidbody2D>();
                        
                        GameObject.Destroy(cvtwheel.GetComponent<Optout>());
                    }
                }
            );
            // Cylinder
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Piston"),
                    NameOverride = "Cylinder - CE",
                    DescriptionOverride = "A cylinder with a piston.",
                    CategoryOverride = ModAPI.FindCategory("Misc."),
                    ThumbnailOverride = ModAPI.LoadSprite("sprites/sp_cylinder.png"),
                    AfterSpawn = (Instance) =>
                    {
                        GameObject.Destroy(Instance.GetComponent<PistonBehaviour>());
                        // Remove the existing slider
                        GameObject.Destroy(Instance.GetComponent<SliderJoint2D>());
                        GameObject.Destroy(Instance.GetComponent<DamagableMachineryBehaviour>());
                        GameObject.Destroy(Instance.GetComponent<MuteBehaviour>());
                                                
                        // Size used by both colliders
                        Vector2 newsize = new Vector2(ModAPI.PixelSize * 59f, ModAPI.PixelSize * 9f);
                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("sprites/sp_cylinder.png");
                        Instance.GetComponent<PhysicalBehaviour>().RefreshOutline();
                        Instance.GetComponent<PhysicalBehaviour>().CalculateCircumference();
                        Instance.GetComponent<PhysicalBehaviour>().BakeColliderGridPoints();
                        Instance.GetComponent<BoxCollider2D>().offset = new Vector2(ModAPI.PixelSize * 0f, ModAPI.PixelSize * -13f);
                        Instance.GetComponent<BoxCollider2D>().size = newsize;
                        // Side 2 collider
                        BoxCollider2D sidebc = Instance.AddComponent<BoxCollider2D>();
                        sidebc.size = newsize;
                        sidebc.offset = new Vector2(ModAPI.PixelSize * 0f, ModAPI.PixelSize * 13f);
                        // Wall collider
                        newsize = new Vector2(ModAPI.PixelSize * 9f, ModAPI.PixelSize * 34f);
                        BoxCollider2D wallbc = Instance.AddComponent<BoxCollider2D>();
                        wallbc.size = newsize;
                        wallbc.offset = new Vector2(ModAPI.PixelSize * 25f, ModAPI.PixelSize * 0f);
                        
                        // Convert the PistonHead into the piston
                        GameObject piston = Instance.transform.Find("PistonHead").gameObject;
                        foreach(Collider2D col in piston.GetComponents<Collider2D>()) GameObject.Destroy(col);
                        BoxCollider2D pistonbc = piston.AddComponent<BoxCollider2D>();
                        pistonbc.size = new Vector2(ModAPI.PixelSize * 27f, ModAPI.PixelSize * 15f);
                        piston.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("sprites/sp_piston.png");
                        piston.GetComponent<PhysicalBehaviour>().RefreshOutline();
                        piston.GetComponent<PhysicalBehaviour>().CalculateCircumference();
                        piston.GetComponent<PhysicalBehaviour>().BakeColliderGridPoints();
                        piston.GetComponent<Transform>().position = Instance.transform.position + (Instance.transform.right * (ModAPI.PixelSize * -8.75f));
                        piston.GetComponent<Transform>().SetParent(Instance.transform);
                        piston.GetComponent<PhysicalBehaviour>().Properties = ModAPI.FindPhysicalProperties("Metal");
                        piston.GetComponent<PhysicalBehaviour>().Properties.BulletSpeedAbsorptionPower = 500f;
                        piston.GetComponent<PhysicalBehaviour>().Properties.Brittleness = 0f;
                        piston.GetComponent<PhysicalBehaviour>().Properties.HeatTransferSpeedMultiplier = 1f;
                        
                        SliderJoint2D slider = piston.AddComponent<SliderJoint2D>();
                        // https://discussions.unity.com/t/modify-slider-joint-2d-via-script/89243/2
                        JointTranslationLimits2D limits = slider.limits;
                        limits.max = ModAPI.PixelSize * 10.5f;
                        limits.min = ModAPI.PixelSize * -6.5f;
                        slider.limits = limits;
                        slider.connectedBody = Instance.GetComponent<Rigidbody2D>();
                        slider.autoConfigureAngle = false;
                        slider.angle = 0f;
                        slider.enableCollision = true;
                        //FrictionJoint2D fj2d = piston.AddComponent<FrictionJoint2D>();
                        //fj2d.connectedBody = Instance.GetComponent<Rigidbody2D>();
                        //fj2d.maxForce = 2000f;
                        
                        // Set the mass of the wall
                        ModResources.SetMass(Instance, 12f);
                        // Set the mass of the piston
                        ModResources.SetMass(piston, 5f);
                    }
                }
            );
            // Signal Wheel
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Metal Wheel"),
                    NameOverride = "Signal Wheel - CE",
                    DescriptionOverride = "A wheel that outputs a small amount of electric charge proportional to its angular velocity. Intended to be used with the Scalable Metal Detector",
                    CategoryOverride = ModAPI.FindCategory("Machinery"),
                    //ThumbnailOverride = ModAPI.LoadSprite("sprites/sp_cylinder.png"),
                    AfterSpawn = (Instance) =>
                    {
                        Instance.AddComponent<AngularVelocitySignalBehaviour>();
                    }
                }
            );
            // Scalable Metal Detector
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Metal Detector"),
                    NameOverride = "Scalable Metal Detector - CE",
                    DescriptionOverride = "Scalable Metal Detector",
                    CategoryOverride = ModAPI.FindCategory("Machinery"),
                    //ThumbnailOverride = ModAPI.LoadSprite("sprites/sp_smallmagnet.png"),
                    AfterSpawn = (Instance) =>
                    {
                        Instance.AddComponent<ScalableDetectorBehaviour>();
                    }
                }
            );
            // Box Pulse Drum
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Pulse Drum"),
                    NameOverride = "Box Pulse Drum - CE",
                    DescriptionOverride = "Pulse drum but in a more convienent rectangle shape",
                    CategoryOverride = ModAPI.FindCategory("Machinery"),
                    ThumbnailOverride = ModAPI.LoadSprite("sprites/sp_boxdrumsmall.png"),
                    AfterSpawn = (Instance) =>
                    {
                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("sprites/sp_boxdrumsmall.png");
                        foreach(Collider2D col in Instance.GetComponents<Collider2D>()) GameObject.Destroy(col);
                        Instance.AddComponent<BoxCollider2D>().size = new Vector2(ModAPI.PixelSize * 26f, ModAPI.PixelSize * 16f);
                        Instance.GetComponent<PhysicalBehaviour>().BakeColliderGridPoints();
                        PulseDrumBehaviour pdb = Instance.GetComponent<PulseDrumBehaviour>();
                        
                        pdb.SpreadAngle = 0.1f;
                        pdb.EmitterSize = 0.2f;
                    }
                }
            );
        }
    }
    public static class ModResources
    {
        public static void SetMass(GameObject Instance, float Mass) {
            var phys = Instance.GetComponent<PhysicalBehaviour>();
            phys.TrueInitialMass = Mass;
            phys.rigidbody.mass = Mass;
            phys.InitialMass = Mass;
        }
        // Angular velocity is presumed to be radians per second
        // See https://forum.unity.com/threads/units-of-rigidbody-angularvelocity.7055/
        public static float angvel2rpm(float angvel) {
            return angvel / (60f / (2f * Mathf.PI));
        }
        public static float rpm2angvel(float rpm) {
            return rpm * (60f / (2f * Mathf.PI));
        }
    }
    // Behavior is spelled as Behaviour to stay consistent with the game's class names
    public class ScalableDetectorBehaviour : MonoBehaviour {
        PhysicalBehaviour phys;
        Activations.NodeBehaviour node;
    
        // 1f = 1 second
        float maxtime = 0.5f;
        float maxcharge = 100f;
        float delay;

        void Start() {
            phys = GetComponent<PhysicalBehaviour>();
            node = GetComponent<Activations.NodeBehaviour>();
        } 
        
        void Update() {
            if (phys.Charge > maxcharge) {
                phys.ActivationPropagationDelay = 0f;
                node.DelaySeconds = 0f;
            } else {
                delay = (maxtime / maxcharge) * phys.Charge;
                phys.ActivationPropagationDelay = delay;
                node.DelaySeconds = delay;
            }
        }
    }
    public class AirBrakeBehaviour : MonoBehaviour {
        Rigidbody2D rb2d;
        PhysicalBehaviour phys;

        float effective_base = 1f;
        float effective_cap = 50000f;

        bool enable;

        void Start() {
            enable = false;
            rb2d = GetComponent<Rigidbody2D>();
            phys = GetComponent<PhysicalBehaviour>();
        }
        void Use(ActivationPropagation pp) {
            enable = !enable;
        }
        void Update() {
            float charge = phys.Charge;
            if (enable) {
                if (charge > effective_cap) {
                    // Set to the effective cap when the charge is past the effective cap
                    rb2d.drag = effective_cap;
                } else if (charge > 0f) {
                    // Up to the effective cap multiply the base by the charge
                    rb2d.drag = effective_base * charge;
                } else {
                    // If no charge then use the base drag value
                    rb2d.drag = effective_base;
                }
            } else {
                // If not active, there is no drag
                rb2d.drag = 0f;
            }
        }
    }
    public class ActResizeBehaviour : MonoBehaviour {
        Vector3 scale;
        Vector3 scalechange;

        void Start() {
            scalechange = new Vector3(-0.1f, -0.1f, -0.1f);
            scale = transform.localScale;
            // Default size
            transform.localScale = new Vector3(1, 1, 1);
        }
        void Use(ActivationPropagation pp) {
            var ch = pp.Channel;
            //Debug.Log(ch);
            // Red
            if (ch == 1) {
                transform.localScale -= scalechange;
                GetComponent<PhysicalBehaviour>().RecalculateMassBasedOnSize();
            }
            // Blue
            // Make sure the item does not get a scale of 0
            else if ((ch == 2) && (transform.localScale.y > 0.4f)) {
                transform.localScale += scalechange;
                GetComponent<PhysicalBehaviour>().RecalculateMassBasedOnSize();
            }
            // Don't do anything if ch == 0 (green)
        }
    }
    public class CVTWheelBehaviour : MonoBehaviour {
        [SkipSerialisation]
        Vector3 scale;
        [SkipSerialisation]
        Rigidbody2D prb2d;
        [SkipSerialisation]
        Rigidbody2D rb2d;
        [SkipSerialisation]
        FixedJoint2D fj2d;
        [SkipSerialisation]
        PhysicalBehaviour pb;
        public GameObject parentobj;
        [SkipSerialisation]
        public float angvelmult = 0.0001f;
        [SkipSerialisation]
        float scalevalue;
        [SkipSerialisation]
        float angvel;
        [SkipSerialisation]
        public float basesize = 0.1f;

        void Start() {
            prb2d = parentobj.GetComponent<Rigidbody2D>();
            rb2d = GetComponent<Rigidbody2D>();
            fj2d = GetComponent<FixedJoint2D>();
            pb = GetComponent<PhysicalBehaviour>();
        }

        void Update() {
            float maxsize = 2f;

            angvel = Mathf.Abs(prb2d.angularVelocity);
            
            scalevalue = ((angvel) * angvelmult) + basesize;

            if (!Global.main.Paused) {
                transform.position = parentobj.transform.position;
            }

            if (scalevalue < maxsize) {
                transform.localScale = new Vector3(scalevalue,scalevalue,1f);
                pb.CalculateCircumference();
            } else {
                transform.localScale = new Vector3(maxsize,maxsize,1f);
            }
        }
    }
    public class CentrifugalClutchBehaviour : MonoBehaviour {
        [SkipSerialisation]
        Rigidbody2D prb2d;
        [SkipSerialisation]
        Rigidbody2D rb2d;
        [SkipSerialisation]
        FixedJoint2D fj2d;
        [SkipSerialisation]
        HingeJoint2D hj2d;
        public GameObject parentobj;
        [SkipSerialisation]
        public float engagevel = ModResources.rpm2angvel(220f);
        [SkipSerialisation]
        public float basesize = 0.1f;
        float angvel;

        void Start() {
            prb2d = parentobj.GetComponent<Rigidbody2D>();
            rb2d = GetComponent<Rigidbody2D>();
            hj2d = GetComponent<HingeJoint2D>();
            fj2d = GetComponent<FixedJoint2D>();
        }

        void Update() {
            angvel = Mathf.Abs(prb2d.angularVelocity);
            
            if (angvel >= engagevel) {
                Debug.Log("engage");
                hj2d.enabled = false;
                fj2d.enabled = true;
            } else {
                Debug.Log("disengage");
                hj2d.enabled = true;
                fj2d.enabled = false;
            }

            if (!Global.main.Paused) {
                transform.position = parentobj.transform.position;
            }
        }
    }
    public class SparkPlugBehaviour : MonoBehaviour {
        FlaskBehaviour fb;
        [SkipSerialisation]
        public AudioClip[] clips = null;
        // Force applied with one liter of oil
        [SkipSerialisation]
        public float baseforce = 600f;
        [SkipSerialisation]
        public float basecapacity = 0.5f;
        // This is for keeping track of how much it can accept and not for customizing base capacity. Defaults to the lowest value of 0.1.
        float capacitymult = 0.1f;
        // Calculated before explosion
        float appforce;
        [SkipSerialisation]
        public Global globalinstance;
        [SkipSerialisation]
        public float explosionoffset = 0.55f;
        [SkipSerialisation]
        public float explosionrange = 0.1f;
        ExplosionCreator.ExplosionParameters ex;
        AudioSource ias;

        void Start() {
            fb = GetComponent<FlaskBehaviour>();
            this.globalinstance = UnityEngine.Object.FindObjectsOfType<Global>()[0];
            
            ex.FragmentationRayCount = 32;
            // VFX is created later
            ex.CreateParticlesAndSound = false;
            ex.BallisticShrapnelCount = 0;
            ex.DismemberChance = 0f;
            
            ias = gameObject.AddComponent<AudioSource>();
            ias.clip = clips[UnityEngine.Random.Range(0, clips.Length)];
            globalinstance.AddAudioSource(ias, false);
        }

        void Use(ActivationPropagation pp) {
            var ch = pp.Channel;
            // Green
            if (ch == 0) {
                if (fb.IsFull()) {
                    // If the base capacity is lower than 1.0f, still have the same amount of force with a full flask
                    appforce = baseforce * ((fb.GetAmount(Liquid.GetLiquid("OIL")) * Liquid.LiquidUnitToLiter) / basecapacity);

                    ex.Position = transform.position + transform.right * explosionoffset;
                    ex.FragmentForce = appforce;
                    ex.Range = explosionrange;

                    ExplosionCreator.Explode(ex);
                    // Add particle effects for a "fake" explosion
                    ModAPI.CreateParticleEffect("Flash", transform.position + transform.right * explosionoffset);
                    ias.Play();

                    fb.ClearLiquid();
                }
            }
            // Red
            else if ((ch == 1) && (capacitymult < 1f)) {
                capacitymult += 0.1f;
            }
            // Blue
            else if ((ch == 2) && (capacitymult >= 0.2f)) {
                capacitymult -= 0.1f;
            }
        }

        void Update() {
            fb.Capacity = (Liquid.LiterToLiquidUnit * basecapacity) * capacitymult;
        }
    }
    public class VariableBatteryBehaviour : MonoBehaviour {
        static float maxcharge = 10f;
        static float mincharge = 0f;
        static float step = 2f;
        float currentcharge = 2f;
        bool active = false;

        PhysicalBehaviour pb;
        void Start() {
            // Replaces GeneratorBehaviour
            Destroy(GetComponent<GeneratorBehaviour>());
            pb = GetComponent<PhysicalBehaviour>();
        }

        void Use(ActivationPropagation pp) {
            var ch = pp.Channel;
            // Green
            if (ch == 0) {
                active = !active;
            }
            // Red
            else if ((ch == 1) && (currentcharge < maxcharge)) {
                currentcharge += step;
            }
            // Blue
            else if ((ch == 2) && (currentcharge > mincharge)) {
                currentcharge -= step;
            }
        }

        void Update() {
            if (active) {
                pb.Charge = currentcharge;
            } else {
                // No charge when not activated
                pb.Charge = 0f;
            }
        }
    }
    public class StabActivationBehaviour : MonoBehaviour {
        PhysicalBehaviour pb;

        void Start() {
            pb = GetComponent<PhysicalBehaviour>();
        }

        void Stabbed() {
            pb.ForceSendUse();
        }
    }
    public class EMPActivationBehaviour : MonoBehaviour {
        PhysicalBehaviour pb;

        void Start() {
            pb = GetComponent<PhysicalBehaviour>();
        }

        void OnEMPHit() {
            // In case a GameObject has both this component and another EMP-related component so it does not loop and crash the game
            if (!((GetComponent<EMPBehaviour>()) || (GetComponent<ScalableEMPBehaviour>()))) {
                pb.ForceSendUse();
            }
        }
    }
    public class ScalableEMPBehaviour : MonoBehaviour {
        PhysicalBehaviour pb;
        EMPBehaviour eb;
        float baserange;
        void Start() {
            pb = GetComponent<PhysicalBehaviour>();
            eb = GetComponent<EMPBehaviour>();
            baserange = eb.Range;
        }
        void Update() {
            float charge_cap = 1000f;

            if (pb.Charge > charge_cap) {
                eb.SetRange(charge_cap * baserange);
            } else if (pb.charge > 0) {
                eb.SetRange(pb.charge * baserange);
            } else {
                eb.SetRange(baserange);
            }
        }
    }
    public class AngularVelocityChargeBehaviour : MonoBehaviour {
        PhysicalBehaviour pb;
        Rigidbody2D rb;
        float angvel;

        // Multiplier
        float mult = 1f;
        // Angular Drag
        float angdrag = 10f;

        void Start() {
            pb = GetComponent<PhysicalBehaviour>();
            rb = GetComponent<Rigidbody2D>();

            rb.angularDrag = angdrag;

        }

        void Update() {
            angvel = rb.angularVelocity;
            pb.Charge = Mathf.Abs(angvel) * mult;
        }
    }
    // Same thing as above but with no (normal amount) drag and a much smaller power output
    public class AngularVelocitySignalBehaviour : MonoBehaviour {
        PhysicalBehaviour pb;
        Rigidbody2D rb;
        float angvel;

        // Multiplier
        float mult = 0.01f;
        // Angular Drag
        float angdrag = 0.05f;

        void Start() {
            pb = GetComponent<PhysicalBehaviour>();
            rb = GetComponent<Rigidbody2D>();

            rb.angularDrag = angdrag;
        }

        void Update() {
            angvel = rb.angularVelocity;
            pb.Charge = Mathf.Abs(angvel) * mult;
        }
    }
    public class ActivationMeterBehaviour : MonoBehaviour {
        public TextMeshPro tm;
        Stack counts = new Stack(3);
        int[] arrcounts = new int[3];
        int sum = 0;
        int counter = 0;
        int avg = 0;

        void Start() {
            StartCoroutine(ActCounter());
        }

        void Update() {
            tm.enabled = true;
            if (counts.Count > 2) {
                avg = 0;
            } else {
                for (int i = 0; i < arrcounts.Length; i++) {
                    sum += arrcounts[i];
                }
                avg = Mathf.RoundToInt(sum / arrcounts.Length);
            }
            tm.text = avg + "/s";
        }

        void Use() {
            counter += 1;
        }

        IEnumerator ActCounter() {
            while (true) {
                counts.Push(counter);
                counter = 0;
                yield return new WaitForSeconds(1.0f);
            }
        }
    }
    public class CartridgeBehaviour : MonoBehaviour {
        [SkipSerialisation]
        public Sprite[] sprites = null;
        [SkipSerialisation]
        public AudioClip[] clips = null;
        [SkipSerialisation]
        PhysicalBehaviour pb;
        [SkipSerialisation]
        public Global globalinstance;
        SpawnableAsset AssetItem;
        GameObject projectile;

        void Start() {
            pb = GetComponent<PhysicalBehaviour>();
            this.globalinstance = UnityEngine.Object.FindObjectsOfType<Global>()[0];
        }

        void Use() {
            // The behavior removes itself so the object cannot explode again
            Destroy (this);

            GetComponent<SpriteRenderer>().sprite = sprites[1];
            Utils.FixColliders(gameObject);
            GetComponent<PhysicalBehaviour>().BakeColliderGridPoints();
            // As the "bullet" left the case, the object should have less weight
            GetComponent<PhysicalBehaviour>().TrueInitialMass = 1f;
            GetComponent<PhysicalBehaviour>().rigidbody.mass = 1f;
            GetComponent<PhysicalBehaviour>().InitialMass = 1f;

            // Create the "bullet" object
            AssetItem = ModAPI.FindSpawnable("Knife");
            projectile = UnityEngine.Object.Instantiate(AssetItem.Prefab, transform.position, transform.rotation);
            // Object is started facing the wrong direction (up if the cartridge is facing right) because idk how to modify sharpaxis
            // After spawning, rotate the object to the correct position
            projectile.GetComponent<Transform>().eulerAngles += new Vector3(0f, 0f, -90f);
            projectile.GetComponent<PhysicalBehaviour>().SpawnSpawnParticles = false;
            projectile.GetComponent<PhysicalBehaviour>().TrueInitialMass = 2f;
            projectile.GetComponent<PhysicalBehaviour>().rigidbody.mass = 2f;
            projectile.GetComponent<PhysicalBehaviour>().InitialMass = 2f;
            projectile.GetComponent<SpriteRenderer>().sprite = sprites[2];
            projectile.GetComponent<PhysicalBehaviour>().RefreshOutline();
            projectile.FixColliders();
            projectile.transform.position += transform.right * 0.2f;

            // Add DebrisComponent so both objects can be removed on "Clear debris"
            projectile.AddComponent<DebrisComponent>();
            gameObject.AddComponent<DebrisComponent>();

            // Instead of an explosion that effect other objects around it, both the case and bullet are pushed in the opposite direction
            projectile.GetComponent<Rigidbody2D>().AddForce(transform.right.normalized * 9600f, ForceMode2D.Force);
            GetComponent<Rigidbody2D>().AddForce(-transform.right.normalized * 9600f, ForceMode2D.Force);
            // Add particle effects for a "fake" explosion
            ModAPI.CreateParticleEffect("Flash", transform.position + transform.right * 0.1f);
            AudioSource ias;
            ias = gameObject.AddComponent<AudioSource>();
            ias.clip = clips[UnityEngine.Random.Range(0, clips.Length)];
            globalinstance.AddAudioSource(ias, false);
            ias.Play();
            globalinstance.RemoveAudioSource(ias);
       }

        void Update() {
            // "electronic" firing
            if (pb.Charge > 10)  {
                 pb.ForceSendUse();
            }
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CreepyTownNPCsMod.NPCs
{
    [AutoloadHead]
    public class Herbalist : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 25;
            NPCID.Sets.AttackFrameCount[NPC.type] = 4;
            NPCID.Sets.DangerDetectRange[NPC.type] = 150;
            NPCID.Sets.AttackType[NPC.type] = 3;
            NPCID.Sets.AttackTime[NPC.type] = 25;
            NPCID.Sets.AttackAverageChance[NPC.type] = 10;
            NPCID.Sets.HatOffsetY[NPC.type] = 4;

            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = -1f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

            NPC.Happiness.SetBiomeAffection<DesertBiome>(AffectionLevel.Hate);
            NPC.Happiness.SetBiomeAffection<ForestBiome>(AffectionLevel.Like);
            NPC.Happiness.SetBiomeAffection<JungleBiome>(AffectionLevel.Love);

            NPC.Happiness.SetNPCAffection(NPCID.TravellingMerchant, AffectionLevel.Hate);
            NPC.Happiness.SetNPCAffection(NPCID.Truffle, AffectionLevel.Dislike);
            NPC.Happiness.SetNPCAffection(NPCID.Dryad, AffectionLevel.Like);
            NPC.Happiness.SetNPCAffection(NPCID.WitchDoctor, AffectionLevel.Love);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,

                new FlavorTextBestiaryInfoElement("An herbalist loves nature. It sells plants and seeds for use and cultivation."),
            });
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.scale = 1.1f;
            NPC.width = 40;
            NPC.height = 40;
            NPC.aiStyle = NPCAIStyleID.Passive;
            NPC.damage = 15;
            NPC.defense = 15;
            NPC.lifeMax = 250;
            NPC.knockBackResist = 0.5f;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            AnimationType = NPCID.Merchant;
        }

        public override bool CanTownNPCSpawn(int numTownNPCs)
        {
            for (int k = 0; k < Main.maxPlayers; k++)
            {
                Player player = Main.player[k];
                if (!player.active)
                {
                    continue;
                }


                if (player.inventory.Any(item => item.type == ItemID.Moonglow))
                {
                    return true;
                }
            }

            return false;
        }

        public override List<string> SetNPCNameList()
        {
            return new List<string>()
            { 
                  ("Plantsen"),
                  ("Evan"),
                  ("Flowy"),
                  ("Cult")
                  /*Language.GetTextValue("Mods.CreepyTownNPCs.NPCNames.Herbalist.Name1"),
                  Language.GetTextValue("Mods.CreepyTownNPCs.NPCNames.Herbalist.Name2"),
                  Language.GetTextValue("Mods.CreepyTownNPCs.NPCNames.Herbalist.Name3"),
                  Language.GetTextValue("Mods.CreepyTownNPCs.NPCNames.Herbalist.Name4")*/
            };
        }

        public override string GetChat()
        {
            switch (Main.rand.Next(4))
            {
                case 0:
                    return ("Don't step on my plants!");
                case 1:
                    return ("There are no magic beanstalks.");
                case 2:
                    return ("Here you will find all the plants you might need.");
                default:
                    return ("Growing plants requires hard work and dedication.");
                /*case 0:
                    return Language.GetTextValue("Mods.CreepyTownNPCs.Dialogue.Herbalist.StandartDialogue1");
                case 1:
                    return Language.GetTextValue("Mods.CreepyTownNPCs.Dialogue.Herbalist.StandartDialogue2");
                case 2:
                    return Language.GetTextValue("Mods.CreepyTownNPCs.Dialogue.Herbalist.StandartDialogue3");
                default:
                    return Language.GetTextValue("Mods.CreepyTownNPCs.Dialogue.Herbalist.StandartDialogue4");*/
            }
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = "Plants";
            button2 = "Seeds";
            /*button = Language.GetTextValue("Mods.CreepyTownNPs.Dialogue.ChatOption1");
            button2 = Language.GetTextValue("Mods.CreepyTownNPs.Dialogue.ChatOption2");*/
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
            if (firstButton)
            {
                shopName = "Plants";
            }
            else
            {
                shopName = "Seeds";
            }
        }

        public override void AddShops()
        {
            NPCShop plantsShop = new NPCShop(NPC.type, "Plants");

            plantsShop.Add(ItemID.Blinkroot);
            plantsShop.Add(ItemID.Daybloom);
            plantsShop.Add(ItemID.Deathweed);
            plantsShop.Add(ItemID.Fireblossom);
            plantsShop.Add(ItemID.Moonglow);
            plantsShop.Add(ItemID.Shiverthorn);
            plantsShop.Add(ItemID.Waterleaf);

            plantsShop.Register();

            NPCShop seedsShop = new NPCShop(NPC.type, "Seeds");

            seedsShop.Add(ItemID.BlinkrootSeeds);
            seedsShop.Add(ItemID.DaybloomSeeds);
            seedsShop.Add(ItemID.DeathweedSeeds);
            seedsShop.Add(ItemID.FireblossomSeeds);
            seedsShop.Add(ItemID.MoonglowSeeds);
            seedsShop.Add(ItemID.ShiverthornSeeds);
            seedsShop.Add(ItemID.WaterleafSeeds);

            seedsShop.Register();
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 15;
            knockback = 6f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 60;
            randExtraCooldown = 25;
        }

        public override void DrawTownAttackSwing(ref Texture2D item, ref Rectangle itemFrame, ref int itemSize, ref float scale, ref Vector2 offset)
        {
            item = ModContent.Request<Texture2D>("Terraria/Images/Item_" + ItemID.Flymeal).Value;

            itemSize = 42;
        }

        public override void TownNPCAttackSwing(ref int itemWidth, ref int itemHeight)
            itemWidth = 35;
            itemHeight = 35;
        }
    }
}

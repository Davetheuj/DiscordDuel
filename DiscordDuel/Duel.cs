using System;
namespace DaVDiscordBot
{
    public static class Duel
    {
        public static string p1Name { get; set; }
        public static int p1Health { get; set; }
        public static int p1Sharks { get; set; }
        public static int p1CombatDoses { get; set; }
        public static float p1Spec { get; set; }
        public static int p1AntiDoses { get; set; }
        public static float p1StatModifier { get; set; }
        public static int p1Karams { get; set; }
        public static int p1PoisonTurns { get; set; }
        public static string p1Discriminator { get; set; }


        public static string p2Name { get; set; }
        public static int p2Health { get; set; }
        public static int p2Sharks { get; set; }
        public static int p2CombatDoses { get; set; }
        public static float p2Spec { get; set; }
        public static int p2AntiDoses { get; set; }
        public static float p2StatModifier { get; set; }
        public static int p2Karams { get; set; }
        public static int p2PoisonTurns { get; set; }
        public static string p2Discriminator { get; set; }

        public static int stake { get; set; }

        public static bool isWhip { get; set; }






        public static int currentPlayerMove;



        public static bool isDueling { get; set; } = false;
        public static bool isRequesting { get; set; } = false;
        public static DateTime requestTime { get; set; }
        public static DateTime lastMoveTime { get; set; }
    }
}

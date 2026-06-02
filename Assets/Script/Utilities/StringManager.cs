using UnityEngine;

public class StringManager : MonoBehaviour
{
    public const string ITEM_BULLET2 = "item-dan-2";
    public const string ITEM_BULLET3 = "PowerUpItem";
    public const string ITEM_BULLET6 = "item-dan-6";
    public const string ITEM_BULLET7 = "item-dan-7";
    public const string ITEM_BULLET8 = "item-dan-8";
    public const string ITEM_BULLET9 = "item-dan-9";
    public const string ITEM_CRYSTAL = "item-crystal";
    public const string ITEM_SHIELD = "item-shield";
    public const string ITEM_ENERGY = "item-energy";
    public const string ITEM_SPEED_ATTACK = "item-speed-attack";
    public const string ITEM_DESTROY_ALL = "item-destroy-all";

    public const string SAVE_POS_LEVEL = "PosLevel";
    public const string SAVE_POS_PLANE = "PosSelectPlane";

    public const string SCENE_MENU = "Menu";
    public const string SCENE_LOADING = "Loading";
    public const string SCENE_SELECTLEVEL = "SelectLevel";
    public const string SCENE_GAMEPLAY = "GamePlay";
    public const string Tip1 = "USE SAVE ME TO CONTINUE THE ADVENTURE";
    public const string Tip2 = "SHIELD IS VERY USEFUL ITEM";
    public const string Tip3 = "YOU CAN SWITCH BETWEEN LEFT OR RIGHT HANDED IN CONFIG";
    public const string Tip4 = "UPGRADE YOUR SPACESHIP TO HIGHER LEVEL THROUGH UPGRADE BUTTON";
    public const string Tip5 = "FIND MORE POWER UP AND SPEED UP ON MISSIONS";
    public const string Tip6 = "PLAY EVERY DAY TO CLAIM REWARDS";
    public const string Tip7 = "COLLECT ITEMS BEFORE HARD WAVES";
    public const string Tip8 = "USE BOOST ITEMS WHEN ENEMY WAVES ARE DENSE";
    public const string Tip9 = "BEST SCORE IS TOTAL BEST SCORE OF ALL LEVEL";
    public const string Tip10 = "UPGRADE YOUR SPACESHIP TO BE STRONGER";

    public const string MapName1 = "JUPITER PLANET";
    public const string MapName2 = "VOLCANO LAND";
    public const string MapName3 = "MECURY PLANET";
    public const string MapName4 = "SATELLITE STATION";
    public const string MapName5 = "ALLIANCE BASE";
    public const string MapName6 = "DESERT";
    public const string MapName7 = "BLUE PLANET";
    public const string MapName8 = "DEAD SEA";

    public const string Slogan1 = "CONQUER THE UNIVERSY";
    public const string Slogan2 = "JOIN OUR SPACESHIP CREW";
    public const string Slogan3 = "SAVE THIS PLANET";
    public const string Slogan4 = "DARK FORCE RISING";
    public const string Slogan5 = "EXPLORE NEW WAYS";

    public const string ContentPlane2 = "Unlock at level 7";
    public const string ContentPlane4 = "Unlock at level 45";
    public const string ContentPlane5 = "Unlock with 90 star";
    public const string ContentPlane6 = "Do you want unlock Thunder Storm?";
    public const string ContentPlane9 = " Unlock at level 35";

    public static class PlayerPrefKeys
    {
        public const string quitTime = "quitTime";
    }
}

public enum TYPE_POPUP
{
    NOT_ENOUGH_GOLD = 1,
    NOT_ENOUGH_CRYSTAL = 2,
    NOT_ENOUGH_ITEM = 3
}

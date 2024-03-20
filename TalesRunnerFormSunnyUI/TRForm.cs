using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalesRunnerFormSunnyUI.About;
using TalesRunnerFormSunnyUI.Equip;
using TalesRunnerFormSunnyUI.Exchange;
using TalesRunnerFormSunnyUI.List;
using TalesRunnerFormSunnyUI.MyRoom;
using TalesRunnerFormSunnyUI.Stat;

namespace TalesRunnerFormSunnyUI.Data
{
    public static class TRForm
    {
        //TODO: 各个窗口交互
        #region 基本变量
        private static FMain FMain;
        private static FAbout FAbout;
        private static FEquip FEquip;
        private static FExchange FExchange;
        private static FList FList;
        private static FMyRoom FMyRoom;
        private static FStat FStat;
        private static FUnbox FUnbox;
        #endregion

        #region 注册窗口
        public static void RegisterFMain(FMain fMain)
        {
            FMain = fMain;
        }

        public static void RegisterFAbout(FAbout fAbout)
        {
            FAbout = fAbout;
        }

        public static void RegisterFEquip(FEquip fEquip)
        {
            FEquip = fEquip;
        }

        public static void RegisterFExchange(FExchange fExchange)
        {
            FExchange = fExchange;
        }

        public static void RegisterFList(FList fList)
        {
            FList = fList;
        }

        public static void RegisterFMyRoom(FMyRoom fMyRoom)
        {
            FMyRoom = fMyRoom;
        }

        public static void RegisterFStat(FStat fStat)
        {
            FStat = fStat;
        }

        public static void RegisterFUnbox(FUnbox fUnbox)
        {
            FUnbox = fUnbox;
        }
        #endregion

        #region FStat
        public static void StatStatusBasicChanged(int value1, int value2, int value3, int value4)
        {
            FMyRoom.SetAttrValue(value1, value2, value3, value4);
        }
        #endregion
    }
}

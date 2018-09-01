using System.Configuration;
using System.Diagnostics;

namespace MulliganWinrate {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    public sealed partial class Settings : ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [UserScopedSettingAttribute()]
        [DebuggerNonUserCodeAttribute()]
        [DefaultSettingValueAttribute("100")]
        public double EnemyScale {
            get {
                return ((double)(this["EnemyScale"]));
            }
            set {
                this["EnemyScale"] = value;
            }
        }
        
        [UserScopedSettingAttribute()]
        [DebuggerNonUserCodeAttribute()]
        [DefaultSettingValueAttribute("100")]
        public double EnemyOpacity {
            get {
                return ((double)(this["EnemyOpacity"]));
            }
            set {
                this["EnemyOpacity"] = value;
            }
        }
        
        [UserScopedSettingAttribute()]
        [DebuggerNonUserCodeAttribute()]
        [DefaultSettingValueAttribute("100")]
        public double FriendlyScale {
            get {
                return ((double)(this["FriendlyScale"]));
            }
            set {
                this["FriendlyScale"] = value;
            }
        }
        
        [UserScopedSettingAttribute()]
        [DebuggerNonUserCodeAttribute()]
        [DefaultSettingValueAttribute("100")]
        public double FriendlyOpacity {
            get {
                return ((double)(this["FriendlyOpacity"]));
            }
            set {
                this["FriendlyOpacity"] = value;
            }
        }
        
        [UserScopedSettingAttribute()]
        [DebuggerNonUserCodeAttribute()]
        [DefaultSettingValueAttribute("0")]
        public double PlayerTop {
            get {
                return ((double)(this["PlayerTop"]));
            }
            set {
                this["PlayerTop"] = value;
            }
        }
        
        [UserScopedSettingAttribute()]
        [DebuggerNonUserCodeAttribute()]
        [DefaultSettingValueAttribute("0")]
        public double EnemyTop {
            get {
                return ((double)(this["EnemyTop"]));
            }
            set {
                this["EnemyTop"] = value;
            }
        }
        
        [UserScopedSettingAttribute()]
        [DebuggerNonUserCodeAttribute()]
        [DefaultSettingValueAttribute("0")]
        public double PlayerLeft {
            get {
                return ((double)(this["PlayerLeft"]));
            }
            set {
                this["PlayerLeft"] = value;
            }
        }
        
        [UserScopedSettingAttribute()]
        [DebuggerNonUserCodeAttribute()]
        [DefaultSettingValueAttribute("0")]
        public double EnemyLeft {
            get {
                return ((double)(this["EnemyLeft"]));
            }
            set {
                this["EnemyLeft"] = value;
            }
        }

        [UserScopedSettingAttribute()]
        [DebuggerNonUserCodeAttribute()]
        [DefaultSettingValueAttribute("True")]
        public bool NormalEnabled {
            get {
                return ((bool)(this["NormalEnabled"]));
            }
            set {
                this["NormalEnabled"] = value;
            }
        }
        
        [UserScopedSettingAttribute()]
        [DebuggerNonUserCodeAttribute()]
        [DefaultSettingValueAttribute("True")]
        public bool EnemyEnabled {
            get {
                return ((bool)(this["EnemyEnabled"]));
            }
            set {
                this["EnemyEnabled"] = value;
            }
        }
         
        
    }
}

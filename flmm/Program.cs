using System;
using System.Windows.Forms;
using System.IO;

namespace fomm {
    class fommException : Exception { public fommException(string msg) : base(msg) { } }

    static class Program {
        public const string Version="0.8.5";
        /*private static string typefromint(int i, bool name) {
            switch(i) {
            case 0x00:
                if(name) return "String"; else return "string";
            case 0x01:
                if(name) return "integer"; else return "int";
            case 0x02:
                if(name) return "float"; else return "float";
            case 0x03:
                if(name) return "object id"; else return "ref";
            case 0x04:
                if(name) return "object reference ID"; else return "ref";
            case 0x05:
                if(name) return "Actor value"; else return "short";
            case 0x06:
                if(name) return "Actor"; else return "ref";
            case 0x07:
                if(name) return "spell item"; else return "ref";
            case 0x08:
                if(name) return "axis"; else return "axis";
            case 0x09:
                if(name) return "Cell"; else return "ref";
            case 0x0a:
                if(name) return "Animation group"; else return "ref";
            case 0x0b:
                if(name) return "magic item"; else return "ref";
            case 0x0c:
                if(name) return "Sound"; else return "ref";
            case 0x0d:
                if(name) return "Topic"; else return "ref";
            case 0x0e:
                if(name) return "Quest"; else return "ref";
            case 0x0f:
                if(name) return "Race"; else return "ref";
            case 0x10:
                if(name) return "Class"; else return "ref";
            case 0x11:
                if(name) return "Fraction"; else return "ref";
            case 0x12:
                if(name) return "Gender"; else return "sex";
            case 0x13:
                if(name) return "Global"; else return "ref";
            case 0x14:
                if(name) return "Furniture"; else return "ref";
            case 0x15:
                if(name) return "object id"; else return "ref";
            case 0x16:
                if(name) return "Variable name"; else return "string";
            case 0x17:
                if(name) return "Stage"; else return "short";
            case 0x18:
                if(name) return "Map marker"; else return "ref";
            case 0x19:
                if(name) return "actor base"; else return "ref";
            case 0x1a:
                if(name) return "Container"; else return "ref";
            case 0x1b:
                if(name) return "World space"; else return "ref";
            case 0x1c:
                if(name) return "Crime type"; else return "short";
            case 0x1d:
                if(name) return "Package"; else return "ref";
            case 0x1e:
                if(name) return "Combat style"; else return "ref";
            case 0x1f:
                if(name) return "Magic effect"; else return "ref";
            case 0x20:
                if(name) return "Form type"; else return "ref";
            case 0x21:
                if(name) return "Weather ID"; else return "ref";
            case 0x23:
                if(name) return "Owner"; else return "ref";
            case 0x24:
                if(name) return "Effect shader ID"; else return "ref";
            case 0x25:
                if(name) return "FormList"; else return "ref";
            case 0x27:
                if(name) return "Perk"; else return "ref";
            case 0x28:
                if(name) return "Note"; else return "ref";
            case 0x29:
                if(name) return "Misc stat"; else return "int";
            case 0x2a:
                if(name) return "Imagespace Modifier ID"; else return "ref";
            case 0x2b:
                if(name) return "Imagespace"; else return "ref";
            case 0x2e:
                if(name) return "Voice type"; else return "ref";
            case 0x2f:
                if(name) return "Encounter zone"; else return "ref";
            case 0x30:
                if(name) return "Idle form"; else return "ref";
            case 0x31:
                if(name) return "Message"; else return "ref";
            case 0x32:
                if(name) return "object ID"; else return "ref";
            case 0x33:
                if(name) return "Alignment"; else return "ref";
            case 0x34:
                if(name) return "Equip type"; else return "ref";
            case 0x35:
                if(name) return "object ID"; else return "ref";
            case 0x36:
                if(name) return "Music"; else return "ref";
            case 0x37:
                if(name) return "Crittical stage"; else return "ref";
            default:
                if(name) return "!!!Unknown!!!"; else return "ref";
            }
        }*/

        private static readonly string tmpPath=Path.Combine(Path.GetTempPath(), "fomm");
        public static readonly string Fallout3SaveDir=Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My games\\Fallout3");
        public static readonly string FOIniPath=Path.Combine(Fallout3SaveDir, "Fallout.ini");
        public static readonly string FOSavesPath=Path.Combine(Fallout3SaveDir, Imports.GetPrivateProfileString("General", "SLocalSavePath", "Games", FOIniPath));

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if(!File.Exists("Fallout3.exe")&&!File.Exists("Fallout3ng.exe")) {
                string path;
                try {
                    path=Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\Software\Bethesda Softworks\Fallout3", "Installed Path", null) as string;
                } catch { path=null; }
                if(path!=null) {
                    Directory.SetCurrentDirectory(path);
                }
            }

            /*StreamWriter sw=new StreamWriter("Functions.xml");
            System.Collections.Generic.List<string> args=new System.Collections.Generic.List<string>();
            string[] lines=File.ReadAllLines("functiondump.txt");
            for(int i=0;i<lines.Length;i++) {
                string[] func=lines[i].Split('\t');
                sw.Write("<Func name=\""+func[1]+"\" opcode=\""+func[0]+"\" ");
                if(func.Length>2&&func[2]!="") sw.Write("short=\""+func[2]+"\" ");
                if(func.Length>3&&func[3]!="") sw.Write("desc=\""+func[3].Replace('"', '\'')+"\" ");
                if(func[1].ToLowerInvariant()=="showmessage") sw.Write("paddingbytes=\"6\" ");
                if(i+1<lines.Length&&lines[i+1][0]=='\t') {
                    bool optional=false;
                    int opcount=0;
                    while(i+1<lines.Length&&lines[i+1][0]=='\t') {
                        i++;
                        func=lines[i].Split('\t');
                        if(func[2]=="1"&&!optional) {
                            optional=true;
                            sw.Write("requiredargs=\""+opcount+"\" ");
                        }
                        opcount++;
                        int argtype=int.Parse(func[1], System.Globalization.NumberStyles.AllowHexSpecifier, null);
                        args.Add("  <Arg name=\""+typefromint(argtype, true)+"\" type=\""+typefromint(argtype, false)+"\" />");
                    }
                    sw.WriteLine(">");
                    foreach(string s in args) sw.WriteLine(s);
                    sw.WriteLine("</Func>");
                    args.Clear();
                } else sw.WriteLine(" />");
            }
            sw.Close();*/

            if(!File.Exists("Fallout3.exe")&&!File.Exists("Fallout3ng.exe")) {
                MessageBox.Show("Could not find fallout 3 directory", "Warning");
                Application.Run(new UtilitiesOnlyForm());
            } else {
                Application.Run(new MainForm());
                //Application.Run(new TESsnip.TESsnip());
            }
            if(Directory.Exists(tmpPath)) Directory.Delete(tmpPath, true);
        }

        internal static string ReadCString(BinaryReader br) {
            string s="";
            while(true) {
                byte b=br.ReadByte();
                if(b==0) return s;
                s+=(char)b;
            }
        }

        internal static bool IsSafeFileName(string s) {
            s=s.Replace('/', '\\');
            if(s.IndexOfAny(Path.GetInvalidPathChars())!=-1) return false;
            if(Path.IsPathRooted(s)) return false;
            if(s.StartsWith(".")||Array.IndexOf<char>(Path.GetInvalidFileNameChars(), s[0])!=-1) return false;
            if(s.Contains("\\..\\")) return false;
            if(s.EndsWith(".")||Array.IndexOf<char>(Path.GetInvalidFileNameChars(), s[s.Length-1])!=-1) return false;
            return true;
        }

        internal static bool IsSafeFolderName(string s) {
            if(s.Length==0) return true;
            s=s.Replace('/', '\\');
            if(s.IndexOfAny(Path.GetInvalidPathChars())!=-1) return false;
            if(Path.IsPathRooted(s)) return false;
            if(s.StartsWith(".")||Array.IndexOf<char>(Path.GetInvalidFileNameChars(), s[0])!=-1) return false;
            if(s.Contains("\\..\\")) return false;
            if(s.EndsWith(".")) return false;
            return true;
        }

        internal static string CreateTempDirectory() {
            string tmp;
            for(int i=0;i<32000;i++) {
                tmp=Path.Combine(tmpPath, i.ToString());
                if(!Directory.Exists(tmp)) {
                    Directory.CreateDirectory(tmp);
                    return tmp+Path.DirectorySeparatorChar;
                }
            }
            throw new fommException("Could not create temp folder because directory is full");
        }
    }
}
/////////////////////////////////////////////////////////////////////////////// 
// Copyright (C) 2003-2014, Open Design Alliance (the "Alliance"). 
// All rights reserved. 
// 
// This software and its documentation and related materials are owned by 
// the Alliance. The software may only be incorporated into application 
// programs owned by members of the Alliance, subject to a signed 
// Membership Agreement and Supplemental Software License Agreement with the
// Alliance. The structure and organization of this software are the valuable  
// trade secrets of the Alliance and its suppliers. The software is also 
// protected by copyright law and international treaty provisions. Application  
// programs incorporating this software must include the following statement 
// with their copyright notices:
//   
//   This application incorporates Teigha(R) software pursuant to a license 
//   agreement with Open Design Alliance.
//   Teigha(R) Copyright (C) 2003-2014 by Open Design Alliance. 
//   All rights reserved.
//
// By use of this software, its documentation or related materials, you 
// acknowledge and accept the above terms.
///////////////////////////////////////////////////////////////////////////////

using Teigha.DatabaseServices;

using Microsoft.Win32;

namespace Ranplan.iBuilding.TeighaApp.AppServices
{
    internal class HostAppServ : HostApplicationServices
    {
        public string FindConfigPath(string configType)
        {
            var subkey = GetRegistryAcadProfilesKey();
            if (subkey.Length > 0)
            {
                subkey += "\\General";
                string searchPath;
                if (GetRegistryString(Registry.CurrentUser, subkey, configType, out searchPath))
                    return searchPath;
            }
            return "";
        }

        private string FindConfigFile(string configType, string file)
        {
            var searchPath = FindConfigPath(configType);
            if (searchPath.Length > 0)
            {
                searchPath = $"{searchPath}\\{file}";
                if (System.IO.File.Exists(searchPath))
                    return searchPath;
            }
            return "";
        }

        public override string FindFile(string file, Database db, FindFileHint hint)
        {
            var sFile = this.FindFileEx(file, db, hint);
            if (sFile.Length > 0)
                return sFile;

            var strFileName = file;
            var ext = strFileName.Length > 3 ? strFileName.Substring(strFileName.Length - 4, 4).ToUpper() : file.ToUpper();
            switch (ext)
            {
                case ".PC3":
                    return FindConfigFile("PrinterConfigDir", file);
                case ".STB":
                case ".CTB":
                    return FindConfigFile("PrinterStyleSheetDir", file);
                case ".PMP":
                    return FindConfigFile("PrinterDescDir", file);
            }

            switch (hint)
            {
                case FindFileHint.FontFile:
                case FindFileHint.CompiledShapeFile:
                case FindFileHint.TrueTypeFontFile:
                case FindFileHint.PatternFile:
                case FindFileHint.FontMapFile:
                case FindFileHint.TextureMapFile:
                    break;
                default:
                    return sFile;
            }

            if (hint != FindFileHint.TextureMapFile && ext != ".SHX" && ext != ".PAT" && ext != ".TTF" && ext != ".TTC")
            {
                strFileName += ".shx";
            }
            else if (hint == FindFileHint.TextureMapFile)
            {
                strFileName.Replace("/", "\\");
                var last = strFileName.LastIndexOf("\\");
                strFileName = last == -1 ? "" : strFileName.Substring(0, last);
            }


            sFile = (hint != FindFileHint.TextureMapFile) ? GetRegistryACADFromProfile() : GetRegistryAVEMAPSFromProfile();
            while (sFile.Length > 0)
            {
                var nFindStr = sFile.IndexOf(";");
                string sPath;
                if (-1 == nFindStr)
                {
                    sPath = sFile;
                    sFile = "";
                }
                else
                {
                    sPath = $"{sFile.Substring(0, nFindStr)}\\{strFileName}";
                    if (System.IO.File.Exists(sPath))
                    {
                        return sPath;
                    }
                    sFile = sFile.Substring(nFindStr + 1, sFile.Length - nFindStr - 1);
                }
            }

            if (hint == FindFileHint.TextureMapFile)
            {
                return sFile;
            }

            if (sFile.Length <= 0)
            {
                var sAcadLocation = GetRegistryAcadLocation();
                if (sAcadLocation.Length > 0)
                {
                    sFile = $"{sAcadLocation}\\Fonts\\{strFileName}";
                    if (System.IO.File.Exists(sFile))
                    {
                        sFile = $"{sAcadLocation}\\Support\\{strFileName}";
                        if (System.IO.File.Exists(sFile))
                        {
                            sFile = "";
                        }
                    }
                }
            }
            return sFile;
        }

        public override bool GetPassword(string fileName, PasswordOptions options, out string pass)
        {
            //PasswordDlg pwdDlg = new PasswordDlg();
            /*pwdDlg.TextFileName.Text = fileName;
            if (pwdDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
              pass = pwdDlg.password.Text;
              return true;
            }*/
            pass = "";
            return false;
        }

        public override string FontMapFileName
        {
            get
            {
                var subkey = GetRegistryAcadProfilesKey();
                if (subkey.Length <= 0)
                    return "";
                subkey += "\\Editor Configuration";
                string fontMapFile;
                if (GetRegistryString(Registry.CurrentUser, subkey, "FontMappingFile", out fontMapFile))
                    return fontMapFile;
                return "";
            }
        }

        bool GetRegistryString(RegistryKey rKey, string subkey, string name, out string value)
        {
            var rv = false;
            object objData = null;

            var regKey = rKey.OpenSubKey(subkey);
            if (regKey != null)
            {
                objData = regKey.GetValue(name);
                if (objData != null)
                {
                    rv = true;
                }
                regKey.Close();
            }
            value = rv ? objData.ToString() : "";

            rKey.Close();
            return rv;
        }

        string GetRegistryAVEMAPSFromProfile()
        {
            var subkey = GetRegistryAcadProfilesKey();
            if (subkey.Length > 0)
            {
                subkey += "\\General";
                // get the value for the ACAD entry in the registry
                string tmp;
                if (GetRegistryString(Registry.CurrentUser, subkey, "AVEMAPS", out tmp))
                    return tmp;
            }
            return "";
        }

        string GetRegistryAcadProfilesKey()
        {
            var subkey = "SOFTWARE\\Autodesk\\AutoCAD";
            string tmp;

            if (!GetRegistryString(Registry.CurrentUser, subkey, "CurVer", out tmp))
                return "";
            subkey += $"\\{tmp}";

            if (!GetRegistryString(Registry.CurrentUser, subkey, "CurVer", out tmp))
                return "";
            subkey += $"\\{tmp}\\Profiles";

            if (!GetRegistryString(Registry.CurrentUser, subkey, "", out tmp))
                return "";
            subkey += $"\\{tmp}";
            return subkey;
        }

        string GetRegistryAcadLocation()
        {
            var subkey = "SOFTWARE\\Autodesk\\AutoCAD";
            string tmp;

            if (!GetRegistryString(Registry.CurrentUser, subkey, "CurVer", out tmp))
                return "";
            subkey += $"\\{tmp}";

            if (!GetRegistryString(Registry.CurrentUser, subkey, "CurVer", out tmp))
                return "";
            subkey += $"\\{tmp}";

            if (!GetRegistryString(Registry.CurrentUser, subkey, "", out tmp))
                return "";
            return tmp;
        }

        string GetRegistryACADFromProfile()
        {
            var subkey = GetRegistryAcadProfilesKey();
            if (subkey.Length > 0)
            {
                subkey += "\\General";
                // get the value for the ACAD entry in the registry
                string tmp;
                if (GetRegistryString(Registry.CurrentUser, subkey, "ACAD", out tmp))
                    return tmp;
            }
            return "";
        }
    };
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using NetParts.Models;
using NetParts.Repositories;

namespace NetParts.Libraries.File
{
    public class ManagerFile
    {
        protected static string pathTemp = null;
        protected static string pathRoot = null;
        
        public ManagerFile(IConfiguration configuration)
        {

        }
        public static bool DeleteImageProduct(string way)
        {
            string Way = Path.Combine(Directory.GetCurrentDirectory(), pathRoot, way.TrimStart('/'));
            if (System.IO.File.Exists(Way))
            {
                System.IO.File.Delete(Way);
                return true;
            }
            else
            {
                return false;
            }
        }
        public static List<Image> MoveImagesProduct(List<string> ListWayTemp, int IdProduct)
        {
            var WayDefinitiveFolderProduct = Path.Combine(Directory.GetCurrentDirectory(), pathRoot, IdProduct.ToString());

            if (!Directory.Exists(WayDefinitiveFolderProduct))
            {
                Directory.CreateDirectory(WayDefinitiveFolderProduct);
            }

            List<Image> ListImagesDef = new List<Image>();
            foreach (var WayTemp in ListWayTemp)
            {
                if (!string.IsNullOrEmpty(WayTemp))
                {
                    var NameFile = Path.GetFileName(WayTemp);
                    var WayDef = Path.Combine(pathRoot, IdProduct.ToString(), NameFile).Replace("\\", "/");
                    if (WayDef != WayTemp)
                    {
                        var WayAbsoluteTemp = Path.Combine(Directory.GetCurrentDirectory(), pathTemp,
                            NameFile);
                        var WayAbsoluteDef = Path.Combine(Directory.GetCurrentDirectory(), pathRoot,
                            IdProduct.ToString(), NameFile);

                        if (System.IO.File.Exists(WayAbsoluteTemp))
                        {
                            if (System.IO.File.Exists(WayAbsoluteDef))
                            {
                                System.IO.File.Delete(WayAbsoluteDef);
                            }
                            System.IO.File.Copy(WayAbsoluteTemp, WayAbsoluteDef);

                            if (System.IO.File.Exists(WayAbsoluteDef))
                            {
                                System.IO.File.Delete(WayAbsoluteTemp);
                            }

                            ListImagesDef.Add(new Image() { Way = Path.Combine(pathRoot, IdProduct.ToString(), NameFile).Replace("\\", "/"), IdProduct = IdProduct });
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        ListImagesDef.Add(new Image() { Way = Path.Combine(pathRoot, IdProduct.ToString(), NameFile).Replace("\\", "/"), IdProduct = IdProduct });
                    }
                }
            }
            return ListImagesDef;
        }

        public static void DeleteImagesProduct(List<Image> ListImage)
        {
            int IdProduct = 0;
            foreach (var Image in ListImage)
            {
                DeleteImageProduct(Image.Way);
                IdProduct = Image.IdProduct;
            }

            var FolderProduct = Path.Combine(Directory.GetCurrentDirectory(), pathRoot, IdProduct.ToString());

            if (Directory.Exists(FolderProduct))
            {
                Directory.Delete(FolderProduct);
            }
        }

        public static string CreateArchiveAssistance(IFormFile file)
        {
            var NameFile = Path.GetFileName(file.FileName);
            var Way = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/archives/temp", NameFile);

            using (var stream = new FileStream(Way, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return Path.Combine("/archives/temp", NameFile).Replace("\\", "/");
        }

        public static bool DeleteArchiveAssistance(string way)
        {
            string Way = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", way.TrimStart('/'));
            if (System.IO.File.Exists(Way))
            {
                System.IO.File.Delete(Way);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static List<Archive> MoveArchivesAssistance(List<string> ListWayTemp, int IdTecAssistance)
        {
            var WayDefinitiveFolderAssistance = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/archives", IdTecAssistance.ToString());
            if (!Directory.Exists(WayDefinitiveFolderAssistance))
            {
                Directory.CreateDirectory(WayDefinitiveFolderAssistance);
            }

            List<Archive> ListArchivesDef = new List<Archive>();
            foreach (var WayTemp in ListWayTemp)
            {
                if (!string.IsNullOrEmpty(WayTemp))
                {
                    var NameFile = Path.GetFileName(WayTemp);
                    var WayDef = Path.Combine("/archives", IdTecAssistance.ToString(), NameFile).Replace("\\", "/");
                    if (WayDef != WayTemp)
                    {
                        var WayAbsoluteTemp = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/archives/temp",
                            NameFile);
                        var WayAbsoluteDef = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/archives",
                            IdTecAssistance.ToString(), NameFile);

                        if (System.IO.File.Exists(WayAbsoluteTemp))
                        {
                            if (System.IO.File.Exists(WayAbsoluteDef))
                            {
                                System.IO.File.Delete(WayAbsoluteDef);
                            }
                            System.IO.File.Copy(WayAbsoluteTemp, WayAbsoluteDef);

                            if (System.IO.File.Exists(WayAbsoluteDef))
                            {
                                System.IO.File.Delete(WayAbsoluteTemp);
                            }

                            ListArchivesDef.Add(new Archive() { Way = Path.Combine("/archives", IdTecAssistance.ToString(), NameFile).Replace("\\", "/"), IdTecAssistance = IdTecAssistance });
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        ListArchivesDef.Add(new Archive() { Way = Path.Combine("/archives", IdTecAssistance.ToString(), NameFile).Replace("\\", "/"), IdTecAssistance = IdTecAssistance });
                    }
                }
            }
            return ListArchivesDef;
        }

        public static void DeleteArchivesAssistance(List<Archive> ListArchive)
        {
            int IdTecAssistance = 0;
            foreach (var Archive in ListArchive)
            {
                DeleteArchiveAssistance(Archive.Way);
                IdTecAssistance = Archive.IdTecAssistance;
            }

            var FolderAssistance = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/archives", IdTecAssistance.ToString());

            if (Directory.Exists(FolderAssistance))
            {
                Directory.Delete(FolderAssistance);
            }
        }
    }
}


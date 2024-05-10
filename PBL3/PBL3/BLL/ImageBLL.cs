﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PBL3.DTO;
using PBL3.DAL;
using System.IO;
using System.Windows.Forms;

namespace PBL3.BLL
{
    public class ImageBLL
    {
        #region ->Singleton Pattern
        private static DataPBL3 db;
        private static ImageBLL _Instance;
        public static ImageBLL Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new ImageBLL();
                return _Instance;
            }
            private set { }
        }
        private ImageBLL()
        {
            db = new DataPBL3();
        }
        #endregion

        public List<string> GetImagePaths(int postID)
        {
            //Lấy ảnh dựa trên post id
            List<string> ls = new List<string>();
            var images = db.Images.Where(image => image.ImageID == postID);
            images.ToList().ForEach(image => ls.Add(image.ImagePath));
            return ls.Take(3).ToList();
        }
        public string GetImageStoragePathsOfPost(int? inforID)
        {
            //Lấy đường dẫn của thư mục lưu trữ ảnh của người dùng có UserID
            if (inforID == null)
                return "";
            string path = Path.GetDirectoryName(Application.ExecutablePath);
            string appPath = Path.GetFullPath(Path.Combine(path, @"..\..\"))
                + @"Resources\Post" + inforID.ToString();
            return appPath;
        }

        #region ->Add/Delete Images
        public void AddImage(string imagePath, int inforID)
        {
            Image image = new Image()
            {
                ImagePath = imagePath,
                InforID = inforID
            };
            db.Images.Add(image);
            db.SaveChanges();
        }
        public void DeleteImageFromFolder(string appPath)
        {
            appPath = appPath + @"\";
            DirectoryInfo d = new DirectoryInfo(appPath);
            FileInfo[] Files = d.GetFiles();
            foreach (FileInfo file in Files)
            {
                File.Delete(appPath + file.Name);
            }
            Directory.Delete(appPath);
        }
        public void DeleteImageFromPost(int inforID)
        {
            List<Image> images = db.Images.Where(i => i.InforID == inforID).ToList();
            images.ForEach(image => db.Images.Remove(image));
            db.SaveChanges();
        }
        #endregion
    }
}

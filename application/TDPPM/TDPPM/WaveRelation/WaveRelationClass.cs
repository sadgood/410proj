using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Drawing;

namespace WaveRelationControl
{
    public class WaveRelationClass
    {
        /// <summary>
        /// Wave 链条的根节点
        /// </summary>
        private ModelClass  _RootModel;

        /// <summary>
        /// 坐标类
        /// </summary>
        private CordClass _CordClass;

        // 建立一个二维变长数组，由于存储各层的节点信息
        // LocationList[0]  表示第一层的节点信息，每一层又使用了一个ArrayList
        // LocationList[1]  表示第二层的节点信息

        ArrayList LocationList = new ArrayList();

        // 已经被添加的节点列表
        Hashtable ExistModelList = new Hashtable();


        //记录M.layer 与 ArrayList的对应关系
        Hashtable HTLayerModel = new Hashtable();


        Queue NeedToAddQueue = new Queue(); 
        



        public WaveRelationClass(ModelClass RootNode)
        {
            _RootModel = RootNode;
        }



        /// <summary>
        /// 获取最大层宽
        /// </summary>
        /// <returns></returns>
        public int GetMaxModelWidth()
        {
            int Depth = GetMaxModelLength();
            int MaxWidth = 0;          


            for (int i = 0; i <= Depth; i++)
            {
                int temp = GetModelWidth(i);
                if (temp > MaxWidth)
                {
                    MaxWidth = temp;
                }
            }
            return MaxWidth;
        }

        /// <summary>
        /// 获取最大宽度所在层
        /// </summary>
        /// <returns></returns>
        private int GetMaxWidthLayer()
        {
            int Depth = GetMaxModelLength();
            int MaxWidth = 0;
            int MaxLayer = 0;


            for (int i = 0; i <= Depth; i++)
            {
                int temp = GetModelWidth(i);
                if (temp > MaxWidth)
                {
                    MaxWidth = temp;
                    MaxLayer = i;
                }
            }

            return MaxWidth;
        }


        /// <summary>
        /// 获取某一层的Wave 节点数,根节点所在层为第0层
        /// </summary>
        /// <param name="Layer">层号</param>
        /// <returns></returns>
        public int GetModelWidth(int Layer)
        {
            ArrayList AL = GetLayerModelList(Layer);
            return AL.Count;
        }

        /// <summary>
        /// 获取某一层的所有子节点,根节点所在层为第0层
        /// </summary>
        /// <param name="Layer"></param>
        /// <returns></returns>
        public ArrayList GetLayerModelList(int Layer)
        {
            ArrayList AL = new ArrayList();
            AL.Add(_RootModel);
            return GetLayerModelList(AL, Layer, 0);
        }

        /// <summary>
        /// 获取某一层的所有子节点
        /// </summary>
        /// <param name="AL"></param>
        /// <param name="Layer"></param>
        /// <param name="CurrentLayer"></param>
        /// <returns></returns>
        private ArrayList GetLayerModelList(ArrayList AL, int Layer, int CurrentLayer)
        {
            if (Layer == CurrentLayer)
            {
                return AL;
            }
            else
            {
                ArrayList ChildList = GetChildModelList(AL);
                CurrentLayer++;
                return GetLayerModelList(ChildList, Layer, CurrentLayer);
            }
        }

        public ArrayList GetChildModelList(ArrayList AL)
        {
            ArrayList modelList = new ArrayList();

            for (int i = 0; i < AL.Count; i++)
            {
                ModelClass MC = (ModelClass)AL[i];

                modelList.AddRange(MC.WaveChild);
                modelList.AddRange(MC.CopyChild);
                
            }

            return modelList;

        }


        public int GetMaxModelLength()
        {
            return GetModelLength(_RootModel, 0);
        }


        /// <summary>
        /// 获取指定节点的Wave 链长度
        /// </summary>
        /// <param name="mClass"></param>
        /// <param name="lastLength"></param>
        /// <returns></returns>
        private int GetModelLength(ModelClass mClass, int lastLength)
        {
            int length = 0;

            if (mClass.WaveChild.Count == 0 && mClass.CopyChild.Count == 0)
            {
                return lastLength + 1;
            }
            else
            {

                for (int i = 0; i < mClass.WaveChild.Count; i++)
                {
                    int temp = GetModelLength((ModelClass)mClass.WaveChild[i], 1);
                    if (temp > length)
                    {
                        length = temp;
                    }
                }


                for (int i = 0; i < mClass.CopyChild.Count; i++)
                {
                    int temp = GetModelLength((ModelClass)mClass.CopyChild[i], 1);
                    if (temp > length)
                    {
                        length = temp;
                    }
                }

                return lastLength + length;
            }

        }

        public void SetCordClass(CordClass cordClass)
        {
            _CordClass = cordClass;
        }

        private void SetLocation(ModelClass modelClass, PointF pointF)
        {
            modelClass.Location = _CordClass.ConvertRealToDraw(pointF);
        }



        /// <summary>
        /// 设置每一个模型所处的行列位置
        /// </summary>
        public void SetEveryModelLocation()
        {

            _RootModel.Layer = 0;
            NeedToAddQueue.Enqueue(_RootModel);

            while (NeedToAddQueue.Count > 0)
            {
                ModelClass MClass = (ModelClass)NeedToAddQueue.Dequeue();
                AddModelToLocation(MClass.Layer, MClass);
            }

            ConvertHTToLocationList();

         }


        private void ConvertHTToLocationList()
        {

            int MinValue = int.MaxValue;


            foreach (int key in HTLayerModel.Keys)
            {
                if (key < MinValue)
                {
                    MinValue = key;
                }
            }
            LocationList.Clear();

            for (int i = 0; i < HTLayerModel.Count; i++)
            {
                int key = MinValue + i;

                ArrayList AL = (ArrayList)HTLayerModel[key];

                LocationList.Add(AL);
            }

        }

         /// <summary>
         /// 添加指定节点及其所有祖先节点到Location 位置中
         /// </summary>
         /// <param name="i"></param>
         /// <param name="M1"></param>
         private void AddModelToLocation(int i, ModelClass M1)
         {
             if (ExistModelList.Contains(M1.ModelID)) 
             {
                 return;
             }

              AMLocation(i,M1);
       
             foreach (ModelClass M in M1.WaveChild)
             {
                 if (!(ExistModelList.Contains(M.ModelID) || NeedToAddQueue.Contains(M)))
                 {
                     M.Layer = i + 1;
                     NeedToAddQueue.Enqueue(M);
                 }
             }

             foreach (ModelClass M in M1.CopyChild)
             {
                 if (!(ExistModelList.Contains(M.ModelID) || NeedToAddQueue.Contains(M)))
                 {
                     M.Layer = i + 1;
                     NeedToAddQueue.Enqueue(M);
                 }
             }

             foreach (ModelClass M in M1.ParentNodeList)
             {
                 if(!(ExistModelList.Contains(M.ModelID) || NeedToAddQueue.Contains(M)))
                 {
                     M.Layer = i - 1;
                     NeedToAddQueue.Enqueue(M);
                 }
             }

        }


        

        private bool CanAddToLocation(ModelClass M1)
        {
            if (!ExistModelList.Contains(M1.ModelID))
            {
                return true;
            }

            return false;

        }

        private void AMLocation(int i,ModelClass M1)
        {
            int AddLayer = 0;

            i += AddLayer;
            if (ExistModelList.Contains(M1.ModelID))
            {
                return;
            }
            else
            {

                ExistModelList.Add(M1.ModelID, M1);
            }

            if (HTLayerModel.ContainsKey(M1.Layer))
            {
                ArrayList AL = (ArrayList)HTLayerModel[M1.Layer];
                AL.Add(M1);
            }
            else
            {
                ArrayList AL = new ArrayList();
                AL.Add(M1);
                HTLayerModel.Add(M1.Layer, AL);
            }
        }


        /// <summary>
        /// 获取位置数组的长度
        /// </summary>
        /// <returns></returns>
        public int GetLocationLength()
        {
            return LocationList.Count;
        }


        /// <summary>
        /// 获取位置节点的宽度
        /// </summary>
        /// <returns></returns>
        public int GetLocationWidth()
        {
            int MaxWidth = 0;

            for (int i = 0; i < LocationList.Count; i++)
            {
                ArrayList AL = (ArrayList)LocationList[i];
                if (AL.Count > MaxWidth)
                {
                    MaxWidth = AL.Count;
                }
            }

            return MaxWidth;
        }



        /// <summary>
        /// 获取当前所有绘图节点
        /// </summary>
        /// <returns></returns>
        public Hashtable GetExistNodeList()
        {
            return ExistModelList;
        }

        /// <summary>
        /// 初始化各节点模型坐标
        /// </summary>
        /// <param name="StartX">起始绘图X坐标</param>
        /// <param name="StartY">起始绘图Y坐标</param>
        /// <param name="PaintWidth">绘图区宽度</param>
        /// <param name="PaintHeight">绘图区高度</param>
        public void InitModelCord(int StartX,int StartY, int PaintWidth, int PaintHeight)
        {
            int WaveLength = GetLocationLength();
            int CanUserHeight = PaintHeight-CtlGlobalClass.UpBlank-CtlGlobalClass.DownBlank;

            int CanUserWidth = PaintWidth - CtlGlobalClass.LeftBlank - CtlGlobalClass.RightBlank;


            for (int i = 0; i < WaveLength; i++)
            {
                ArrayList LayerModelList = (ArrayList)LocationList[i];
                int ModelCount = LayerModelList.Count;

                if (ModelCount == 1)
                {
                    ModelClass MClass = (ModelClass)LayerModelList[0];
                    //MClass.Location.X = CtlGlobalClass.LeftBlank + CtlGlobalClass.HLayerDis * i + StartX;
                    //MClass.Location.Y = CtlGlobalClass.UpBlank + CanUserHeight / 2 + StartY;

                    MClass.Location.X = CtlGlobalClass.LeftBlank + CanUserWidth/2  + StartX;
                    MClass.Location.Y = CtlGlobalClass.UpBlank + i * CtlGlobalClass.VShapeDis + StartY;
                }
                else if ((ModelCount % 2) == 1)
                {
                    int Temp = ModelCount / 2;

                    for (int j = 0; j < ModelCount; j++)
                    {
                        ModelClass MClass = (ModelClass)LayerModelList[j];
                        //MClass.Location.X = CtlGlobalClass.LeftBlank + CtlGlobalClass.HLayerDis * i + StartX;
                        //MClass.Location.Y = CtlGlobalClass.UpBlank + CanUserHeight / 2 + (j - Temp) * CtlGlobalClass.VShapeDis + StartY;
                        MClass.Location.Y = CtlGlobalClass.UpBlank + CtlGlobalClass.VShapeDis * i + StartY;
                        MClass.Location.X = CtlGlobalClass.LeftBlank + CanUserWidth / 2 + (j - Temp) * CtlGlobalClass.HLayerDis + StartX;
                    }
                }
                else
                {
                    int Temp = ModelCount / 2;

                    for (int j = 0; j < ModelCount; j++)
                    {
                        ModelClass MClass = (ModelClass)LayerModelList[j];
                        //MClass.Location.X = CtlGlobalClass.LeftBlank + CtlGlobalClass.HLayerDis * i + StartX;
                        //double Tempd = ((double)j - Temp + 0.5) * CtlGlobalClass.VShapeDis;
                        //MClass.Location.Y = CtlGlobalClass.UpBlank + CanUserHeight / 2 + (int)Tempd + StartY;

                        double Tempd = ((double)j - Temp + 0.5) * CtlGlobalClass.HLayerDis;
                        MClass.Location.Y = MClass.Location.Y = CtlGlobalClass.UpBlank + CtlGlobalClass.VShapeDis * i + StartY;
                        MClass.Location.X = CtlGlobalClass.LeftBlank + CanUserWidth / 2 + (int)Tempd + StartX;
                    }
                }
            
           }
       
        }

        public void DrawModel(Graphics g)
        {
            for (int i = 0; i < LocationList.Count; i++)
            {
                ArrayList AL = (ArrayList)LocationList[i];

                for (int j = 0; j < AL.Count; j++)
                {
                    ModelClass M = (ModelClass)AL[j];
                    M.DrawModel(g);
                }
            }
        }


    }
}

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
        /// Wave �����ĸ��ڵ�
        /// </summary>
        private ModelClass  _RootModel;

        /// <summary>
        /// ������
        /// </summary>
        private CordClass _CordClass;

        // ����һ����ά�䳤���飬���ڴ洢����Ľڵ���Ϣ
        // LocationList[0]  ��ʾ��һ��Ľڵ���Ϣ��ÿһ����ʹ����һ��ArrayList
        // LocationList[1]  ��ʾ�ڶ���Ľڵ���Ϣ

        ArrayList LocationList = new ArrayList();

        // �Ѿ�����ӵĽڵ��б�
        Hashtable ExistModelList = new Hashtable();


        //��¼M.layer �� ArrayList�Ķ�Ӧ��ϵ
        Hashtable HTLayerModel = new Hashtable();


        Queue NeedToAddQueue = new Queue(); 
        



        public WaveRelationClass(ModelClass RootNode)
        {
            _RootModel = RootNode;
        }



        /// <summary>
        /// ��ȡ�����
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
        /// ��ȡ��������ڲ�
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
        /// ��ȡĳһ���Wave �ڵ���,���ڵ����ڲ�Ϊ��0��
        /// </summary>
        /// <param name="Layer">���</param>
        /// <returns></returns>
        public int GetModelWidth(int Layer)
        {
            ArrayList AL = GetLayerModelList(Layer);
            return AL.Count;
        }

        /// <summary>
        /// ��ȡĳһ��������ӽڵ�,���ڵ����ڲ�Ϊ��0��
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
        /// ��ȡĳһ��������ӽڵ�
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
        /// ��ȡָ���ڵ��Wave ������
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
        /// ����ÿһ��ģ������������λ��
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
         /// ���ָ���ڵ㼰���������Ƚڵ㵽Location λ����
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
        /// ��ȡλ������ĳ���
        /// </summary>
        /// <returns></returns>
        public int GetLocationLength()
        {
            return LocationList.Count;
        }


        /// <summary>
        /// ��ȡλ�ýڵ�Ŀ��
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
        /// ��ȡ��ǰ���л�ͼ�ڵ�
        /// </summary>
        /// <returns></returns>
        public Hashtable GetExistNodeList()
        {
            return ExistModelList;
        }

        /// <summary>
        /// ��ʼ�����ڵ�ģ������
        /// </summary>
        /// <param name="StartX">��ʼ��ͼX����</param>
        /// <param name="StartY">��ʼ��ͼY����</param>
        /// <param name="PaintWidth">��ͼ�����</param>
        /// <param name="PaintHeight">��ͼ���߶�</param>
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

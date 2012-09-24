using System;
using System.Collections.Generic;
using System.Text;

namespace WaveRelationControl
{
    class TempClass
    {
        private void Temp()
        {


            //int MaxWaveLength = GetMaxModelLength();
            //int MaxModelDepth = GetMaxModelWidth();

            // AddModelToLocation(0, _RootModel);
            //    // 添加第一层的节点信息
            //    //ArrayList FirstArrayList = new ArrayList();
            //    //FirstArrayList.Add(_RootModel);
            //    //ExistModelList.Add(_RootModel.ModelID,_RootModel);
            //    //LocationList.Add(FirstArrayList);

            //    // 添加标准树上的所有节点到位置节点
            //    for (int i = 0; i < MaxWaveLength; i++)
            //    {
            //        ArrayList AL = new ArrayList();
            //        ArrayList LayerModelList = GetLayerModelList(i);
            //        int ModelCount = LayerModelList.Count;


            //        // 循环添加第 i 层的节点 到 AL，同时排除重复节点
            //        for (int j = 0; j < ModelCount; j++)
            //        {
            //            ModelClass M = (ModelClass)LayerModelList[j];
            //            int ModelID = M.ModelID;

            //            if (!ExistModelList.Contains(ModelID))
            //            {
            //                ExistModelList.Add(ModelID,M);
            //                AL.Add(M);
            //            }
            //        }

            //        if (AL.Count != 0)
            //        {
            //            // 将第i层的位置节点添加到位置节点中
            //            LocationList.Add(AL);
            //        }

            //    }


            //    // 添加非树形结构节点
            //    for (int i = 0; i < MaxWaveLength; i++)
            //    {
            //        ArrayList AL = new ArrayList();
            //        ArrayList LayerModelList = GetLayerModelList(i);
            //        int ModelCount = LayerModelList.Count;


            //        // 循环添加第 i 层的节点 到 AL，同时排除重复节点
            //        for (int j = 0; j < LayerModelList.Count; j++)
            //        {
            //            ModelClass M = (ModelClass)LayerModelList[j];

            //            foreach (ModelClass M1 in M.ParentNodeList)
            //            {
            //                if (!ExistModelList.Contains(M1.ModelID))
            //                {
            //                    AddModelToLocation(i-1, M1);
            //                }
            //            }

            //        }

            //        //// 将第i层的位置节点添加到位置节点中
            //        //LocationList.Add(AL);

            //    }
        }
    }
}

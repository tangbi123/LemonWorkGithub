using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Lemon.audio
{

    class ClipsManager
    {
        //从配置文件中加载 Clips文件
        string[] clipNames;
        SingleClip[] allSingleClips;

        public ClipsManager()
        {
            ReadConfig();
            LoadClips();
        }

        /// <summary>
        /// 读取配置文件
        /// 文件需要放在 StreamingAssets文件下
        /// </summary>
        public void ReadConfig()
        {

            var fileAddress = System.IO.Path.Combine(
                Application.streamingAssetsPath, "ClipNames.txt");
            FileInfo file = new FileInfo(fileAddress);

            string s = "";
            if (file.Exists)
            {
                StreamReader r = new StreamReader(fileAddress);

                string tempLine = "";
                
                tempLine = r.ReadLine();//读取多少行
                
                int lineCount = 0;
                if (int.TryParse(tempLine, out lineCount))
                {
                    //Debug.Log("lineCount = " + lineCount);

                    clipNames = new string[lineCount];
                    for (int i = 0; i < lineCount; i++)
                    {
                        tempLine = r.ReadLine();
                        //分割数组
                        string[] splits = tempLine.Split(" ".ToCharArray());

                        clipNames[i] = splits[0];
                        //Debug.Log(clipNames[i]);
                    }

                }
                r.Close();
            }
            else Debug.Log("can not find wenjian ");
            
        }

        /// <summary>
        /// 加载clips到内存
        /// 加载的文件需放在 Resources 文件下 ，并且不需要后缀
        /// </summary>
        public void LoadClips()
        {
            allSingleClips = new SingleClip[clipNames.Length];
            for (int i = 0; i < clipNames.Length; i++)
            {
                //增加目录；
                AudioClip tempClip = Resources.Load<AudioClip>("Sounds/" + clipNames[i]);
                Debug.Log(tempClip.name);
                SingleClip tempSingle = new SingleClip(tempClip);
                allSingleClips[i] = tempSingle;
            }
        }

        public SingleClip FindClipByName(string clipName)
        {
            int tempIndex = -1;
            for (int i = 0; i < clipNames.Length; i++)
            {
                if(clipNames[i].Equals(clipName))
                {
                    tempIndex = i;
                    break;
                }
            }

            if (tempIndex != -1)
            {
                return allSingleClips[tempIndex];

            }
            else
            {
                Debug.Log("can not find the clip");
                return null;
            }
        }
    }
}

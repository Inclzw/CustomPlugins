namespace PhotoshopFile
{
    internal class GuideLineInfo : ImageResource
    {
        public override ResourceID ID => ResourceID.GridGuidesInfo;
        public uint Version { get; set; }
        public uint RowNum { get; set; }
        public uint ColNum { get; set; }
        public uint AllNum { get; set; }
        public readonly float[] simpleCoord = new float[2];
        public readonly SingleGuide[] singleGuideList;

        public GuideLineInfo(PsdBinaryReader reader, string name, int resourceDataLength) : base(name)
        {
            var endPosition = reader.BaseStream.Position + resourceDataLength;

            Version = reader.ReadUInt32();
            RowNum = reader.ReadUInt32();
            ColNum = reader.ReadUInt32();
            AllNum = reader.ReadUInt32();
            if (AllNum <= 0)
            {
                return;
            }

            singleGuideList = new SingleGuide[AllNum];
            var index = 0;
            while (reader.BaseStream.Position < endPosition)
            {
                float cor = reader.ReadUInt32() >> 5; // 读取之后要转成小数，右移5位
                var lineType = reader.ReadByte();
                singleGuideList[index] = new SingleGuide()
                {
                    Cor = cor,
                    LineType = lineType
                };
                index++;
                if (lineType == 0)
                {
                    simpleCoord[0] = cor;
                }
                else if (lineType == 1)
                {
                    simpleCoord[1] = cor;
                }
            }
        }

        public class SingleGuide
        {
            public float Cor { get; set; }
            public byte LineType { get; set; } // 参考线类型,0为垂直网格线,1为水平网格线
        }
    }
}
using System.Collections;
using System.Collections.Generic;

namespace AssemblyCSharp
{
    public class EntityInfo
    {
        private string name;
        private string type;
        private bool isPlayer;
        private bool isEnemy;
        private bool isStatic;
        private bool isGenerator;
        private ArrayList componentsInfo;

        public EntityInfo(string name, string type, bool isPlayer, bool isEnemy, ArrayList componentsInfo, bool isGenerator)
        {
            this.name = name;
            this.type = type;
            this.isPlayer = isPlayer;
            this.isEnemy = isEnemy;
            this.isGenerator = isGenerator;
            this.componentsInfo = componentsInfo;
            
        }

        public string getName
        {
            get { return name; }
        }

        public string getType
        {
            get { return type; }
        }

        public bool getIsPlayer
        {
            get { return isPlayer; }
        }

        public bool getIsEnemy
        {
            get { return isEnemy; }
        }
                
        public bool getIsGenerator
        {
            get { return isGenerator; }
        }

        public ArrayList getComponentsInfo
        {
            get { return componentsInfo; }
        }
    }
}


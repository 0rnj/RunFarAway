namespace CodeBase.Core.Services.ResourcesLoading
{
    public struct ResourceData
    {
        public bool DontDestroy;
        public string Name;
        
        
        public ResourceData(bool dontDestroy, string name)
        {
            DontDestroy = dontDestroy;
            Name = name;
        }
    }
}
namespace TenMaker.Core.Save
{
    public interface IRepository<T>
    {
        void SetData(T data);
        /// <summary>
        /// 복사본 반환
        /// </summary>
        T GetData();
    }
}
namespace Node.Events
{
    public delegate void DataQuantityEventArgsEvemtHandler(object sender, DataQuantityEventArgs args);
    
    public class DataQuantityEventArgs
    {
        public long Qunatity { get; set; }

        public DataQuantityEventArgs(long qunatity)
        {
            Qunatity = qunatity;
        }
    }
}
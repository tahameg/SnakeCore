namespace TahaCore.Runtime.Config.Types
{
    public class Guid : ConfigType<System.Guid>
    {
        /// <inheritdoc cref="ConfigType{T}(string)"/>
        public Guid(string value) : base(value){}
        
        
        /// <inheritdoc cref="ConfigType{T}()"/>
        public Guid(){}
        protected override System.Guid Parse(string value)
        {
            if(value == null) return default;
            string trimmedValue = value.Trim();
            return System.Guid.Parse(trimmedValue);
        }
    }
}
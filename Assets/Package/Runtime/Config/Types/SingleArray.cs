using System;

namespace TahaCore.Config.Types
{
    /// <summary>
    /// Container for Single precision floating point number arrays. Use this to parse a config value to Single array.<br/>
    /// Format is as follows: [1.0, 2.0, 3.0] or [1, 2, 3]<br/>
    /// Rules: <br/>
    /// - Entry must be enclosed in square brackets.<br/>
    /// </summary>
    public class SingleArray : ArrayConfigType<float>
    {
        /// <inheritdoc cref="ArrayConfigType{T}(string)"/>
        public SingleArray(string value) : base(value){}
        
        /// <inheritdoc cref="ArrayConfigType{T}()"/>
        public SingleArray(){}
        
        /// <inheritdoc cref="ArrayConfigType{T}.ParseItem(string)" />
        protected override float ParseItem(string value)
        {
            if(value == null) return default;
            try
            {
                return Convert.ToSingle(value.Trim());
            }
            catch (Exception)
            {
                throw new FormatException("Could not parse value to Single");
            }
        }
    }
}
using System;
using System.Collections.Generic;

namespace HidSharp
{
    /// <summary>
    /// Describes all options for opening a device stream.
    /// </summary>
    public class OpenConfiguration : ICloneable
    {
        Dictionary<OpenOption, object> _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenConfiguration"/> class.
        /// </summary>
        public OpenConfiguration()
        {
            _options = new Dictionary<OpenOption, object>();
        }

        OpenConfiguration(Dictionary<OpenOption, object> options)
        {
            _options = new Dictionary<OpenOption, object>(options);
        }

        public OpenConfiguration Clone()
        {
            return new OpenConfiguration(_options);
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        /// <summary>
        /// Gets the current value of an option.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <returns>The option's value.</returns>
        public object GetOption(OpenOption option)
        {
            Throw.If.Null(option, "option");

            object value;
            return _options.TryGetValue(option, out value) ? value : option.DefaultValue;
        }

        /// <summary>
        /// Gets a list of all currently set options.
        /// </summary>
        /// <returns>The options list.</returns>
        public IEnumerable<OpenOption> GetOptionsList()
        {
            return _options.Keys;
        }

        /// <summary>
        /// Checks if an option has been set.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <returns><c>true</c> if the option has been set.</returns>
        public bool IsOptionSet(OpenOption option)
        {
            Throw.If.Null(option, "option");

            return _options.ContainsKey(option);
        }

        /// <summary>
        /// Sets the current value of an option.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <param name="value">The value to set it to.</param>
        public void SetOption(OpenOption option, object value)
        {
            Throw.If.Null(option, "option");

            if (value != null)
            {
                _options[option] = value;
            }
            else
            {
                _options.Remove(option);
            }
        }
    }
}

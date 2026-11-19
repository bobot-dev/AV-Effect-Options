using System;

namespace AVOptions.Configurations

{
    class ConfigurationException(string message, Exception ex) : Exception(message, ex);
}

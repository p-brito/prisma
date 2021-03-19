﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Prisma.Core.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Prisma.Core.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Argument cannot be empty..
        /// </summary>
        internal static string RES_Exception_ArgCannotBeEmpty {
            get {
                return ResourceManager.GetString("RES_Exception_ArgCannotBeEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Argument cannot be null..
        /// </summary>
        internal static string RES_Exception_ArgCannotBeNull {
            get {
                return ResourceManager.GetString("RES_Exception_ArgCannotBeNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Argument cannot be null or empty..
        /// </summary>
        internal static string RES_Exception_ArgCannotBeNullOrEmpty {
            get {
                return ResourceManager.GetString("RES_Exception_ArgCannotBeNullOrEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The {0} does not exist in the provided configuration..
        /// </summary>
        internal static string RES_Exception_ConfigString_MissingKey {
            get {
                return ResourceManager.GetString("RES_Exception_ConfigString_MissingKey", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An error occurred while trying to delete  the entity the id {0}..
        /// </summary>
        internal static string RES_Exception_Delete {
            get {
                return ResourceManager.GetString("RES_Exception_Delete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An error occurred while trying to get the the entity the id {0}..
        /// </summary>
        internal static string RES_Exception_Get {
            get {
                return ResourceManager.GetString("RES_Exception_Get", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An error occurred while trying to initialize the {0} provider..
        /// </summary>
        internal static string RES_Exception_Initializing_Provider {
            get {
                return ResourceManager.GetString("RES_Exception_Initializing_Provider", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An error occurred while trying to insert the  the entity the id {0}..
        /// </summary>
        internal static string RES_Exception_Insert {
            get {
                return ResourceManager.GetString("RES_Exception_Insert", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An error occurred while trying to update the entity the id {0}..
        /// </summary>
        internal static string RES_Exception_Update {
            get {
                return ResourceManager.GetString("RES_Exception_Update", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An error occurred while trying to update the specified entity. The entity with the id {0} does not exist..
        /// </summary>
        internal static string RES_Exception_Update_EntityDoesNotExist {
            get {
                return ResourceManager.GetString("RES_Exception_Update_EntityDoesNotExist", resourceCulture);
            }
        }
    }
}
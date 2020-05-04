using System;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace Syroot.Windows.IO
  {
  /// <summary>
  /// Represents a special Windows directory and provides methods to retrieve information about it.
  /// </summary>
  public sealed class KnownFolder
    {
    // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

    /// <summary>
    /// Initializes a new instance of the <see cref="KnownFolder"/> class for the folder of the given type. It
    /// provides the values for the current user.
    /// </summary>
    /// <param name="Type">The <see cref="KnownFolderType"/> of the known folder to represent.</param>
    public KnownFolder(KnownFolderType Type)
        : this(Type, WindowsIdentity.GetCurrent())
      {
      }

    /// <summary>
    /// Initializes a new instance of the <see cref="KnownFolder"/> class for the folder of the given type. It
    /// provides the values for the given impersonated user.
    /// </summary>
    /// <param name="Type">The <see cref="KnownFolderType"/> of the known folder to represent.</param>
    /// <param name="Identity">The <see cref="WindowsIdentity"/> of the impersonated user which values will be
    /// provided.</param>
    public KnownFolder(KnownFolderType Type, WindowsIdentity Identity)
      {
      this.Type = Type;
      this.Identity = Identity;
      }

    // ---- PROPERTIES ---------------------------------------------------------------------------------------------

    /// <summary>
    /// Gets the type of the known folder which is represented.
    /// </summary>
    public KnownFolderType Type
      {
      get;
      private set;
      }

    /// <summary>
    /// Gets the <see cref="WindowsIdentity"/> of the user whose folder values are provided.
    /// </summary>
    public WindowsIdentity Identity
      {
      get;
      private set;
      }

    /// <summary>
    /// Gets or sets the default path of the folder.
    /// This does not require the folder to be existent.
    /// </summary>
    /// <exception cref="ExternalException">The known folder could not be retrieved.</exception>
    public string DefaultPath => GetPath(KnownFolderFlags.DontVerify | KnownFolderFlags.DefaultPath);

    /// <summary>
    /// Gets or sets the path as currently configured.
    /// This does not require the folder to be existent.
    /// </summary>
    /// <exception cref="ExternalException">The known folder could not be retrieved.</exception>
    public string Path
      {
      get => GetPath(KnownFolderFlags.DontVerify);
      set => SetPath(KnownFolderFlags.None, value);
      }

    /// <summary>
    /// Gets or sets the path as currently configured, with all environment variables expanded.
    /// This does not require the folder to be existent.
    /// </summary>
    /// <exception cref="ExternalException">The known folder could not be retrieved.</exception>
    public string ExpandedPath
      {
      get => GetPath(KnownFolderFlags.DontVerify | KnownFolderFlags.NoAlias);
      set => SetPath(KnownFolderFlags.DontUnexpand, value);
      }

    // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

    /// <summary>
    /// Creates the folder using its Desktop.ini settings.
    /// </summary>
    /// <exception cref="ExternalException">The known folder could not be retrieved.</exception>
    public void Create()
      {
      GetPath(KnownFolderFlags.Init | KnownFolderFlags.Create);
      }

    // ---- METHODS (PRIVATE) --------------------------------------------------------------------------------------

    private string GetPath(KnownFolderFlags Flags)
      {
      int Result = SHGetKnownFolderPath(Type.GetGuid(), (uint)Flags, Identity.Token, out IntPtr OutPath);
      if (Result >= 0)
        {
        return Marshal.PtrToStringUni(OutPath);
        }
      else
        {
        throw new ExternalException("Cannot get the known folder path. It may not be available on this system.",
            Result);
        }
      }

    private void SetPath(KnownFolderFlags Flags, string Path)
      {
      int Result = SHSetKnownFolderPath(Type.GetGuid(), (uint)Flags, Identity.Token, Path);
      if (Result < 0)
        {
        throw new ExternalException("Cannot set the known folder path. It may not be available on this system.",
            Result);
        }
      }

    /// <summary>
    /// Retrieves the full path of a known folder identified by the folder's known folder ID.
    /// </summary>
    /// <param name="Rfid">A known folder ID that identifies the folder.</param>
    /// <param name="DwFlags">Flags that specify special retrieval options. This value can be 0; otherwise, one or
    /// more of the <see cref="KnownFolderFlags"/> values.</param>
    /// <param name="HToken">An access token that represents a particular user. If this parameter is NULL, which is
    /// the most common usage, the function requests the known folder for the current user. Assigning a value of -1
    /// indicates the Default User. The default user profile is duplicated when any new user account is created.
    /// Note that access to the Default User folders requires administrator privileges.</param>
    /// <param name="PpszPath">When this method returns, contains the address of a string that specifies the path of
    /// the known folder. The returned path does not include a trailing backslash.</param>
    /// <returns>Returns S_OK if successful, or an error value otherwise.</returns>
    /// <msdn-id>bb762188</msdn-id>
    [DllImport("Shell32.dll")]
    private static extern int SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)]Guid Rfid, uint DwFlags,
        IntPtr HToken, out IntPtr PpszPath);

    /// <summary>
    /// Redirects a known folder to a new location.
    /// </summary>
    /// <param name="Rfid">A <see cref="Guid"/> that identifies the known folder.</param>
    /// <param name="DwFlags">Either 0 or <see cref="KnownFolderFlags.DontUnexpand"/>.</param>
    /// <param name="HToken"></param>
    /// <param name="PszPath"></param>
    /// <returns></returns>
    /// <msdn-id>bb762249</msdn-id>
    [DllImport("Shell32.dll")]
    private static extern int SHSetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)]Guid Rfid, uint DwFlags,
        IntPtr HToken, [MarshalAs(UnmanagedType.LPWStr)]string PszPath);

    // ---- ENUMERATIONS -------------------------------------------------------------------------------------------

    /// <summary>
    /// Represents the retrieval options for known folders.
    /// </summary>
    /// <msdn-id>dd378447</msdn-id>
    [Flags]
    private enum KnownFolderFlags : uint
      {
      None = 0x00000000,
      SimpleIdList = 0x00000100,
      NotParentRelative = 0x00000200,
      DefaultPath = 0x00000400,
      Init = 0x00000800,
      NoAlias = 0x00001000,
      DontUnexpand = 0x00002000,
      DontVerify = 0x00004000,
      Create = 0x00008000,
      NoAppcontainerRedirection = 0x00010000,
      AliasOnly = 0x80000000
      }
    }
  }

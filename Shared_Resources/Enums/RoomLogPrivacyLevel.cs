namespace Shared_Resources.Enums;

public enum RoomLogPrivacyLevel
{
    Public, // everybody
    Discrete, // Reveeled by camera, human, mush, 
    Covert, // Reveealed by camera
    Cultist, // only visiblein the mush channel
    Private, // visible only to the owner
    Invisible // action that leaves no log

}

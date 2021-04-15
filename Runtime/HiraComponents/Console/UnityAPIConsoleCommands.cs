using UnityEngine;
using UnityEngine.Scripting;

namespace HiraEngine.Components.Console.Internal
{
    [Preserve, HiraConsoleType("", true)]
    public static class UnityAPIConsoleCommands
    {
        [Preserve, HiraConsoleCallable("Application.Quit")]
        private static void ApplicationQuit() => Application.Quit();

        [Preserve, HiraConsoleCallable("Application.QuitWithExitCode")]
        private static void ApplicationQuit(int code) => Application.Quit(code);

        [Preserve, HiraConsoleCallable("Cursor.visible")]
        private static void CursorVisible(bool value) => Cursor.visible = value;

        [Preserve, HiraConsoleCallable("Cursor.unlock")]
        private static void CursorUnlock() => Cursor.lockState = CursorLockMode.None;

        [Preserve, HiraConsoleCallable("Cursor.lock")]
        private static void CursorLock() => Cursor.lockState = CursorLockMode.Locked;

        [Preserve, HiraConsoleCallable("Cursor.confine")]
        private static void CursorConfine() => Cursor.lockState = CursorLockMode.Confined;

        [Preserve, HiraConsoleCallable("Physics.gravity")]
        private static void PhysicsGravity(Vector3 value) => Physics.gravity = value;

        [Preserve, HiraConsoleCallable("Time.timeScale")]
        private static void TimeTimeScale(float value) => Time.timeScale = value;
    }
}
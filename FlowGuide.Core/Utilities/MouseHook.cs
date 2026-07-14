using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;

namespace FlowGuide.Core.Utilities;

public class MouseHook : IDisposable
{
    public event EventHandler<Point>? MouseMove;
    public event EventHandler<Point>? LeftButtonDown;
    public event EventHandler<Point>? LeftButtonUp;
    public event EventHandler<Point>? RightButtonDown;
    public event EventHandler<Point>? RightButtonUp;

    private const int WH_MOUSE_LL = 14;
    
    private const int WM_MOUSEMOVE = 0x0200;
    private const int WM_LBUTTONDOWN = 0x0201;
    private const int WM_LBUTTONUP = 0x0202;
    private const int WM_RBUTTONDOWN = 0x0204;
    private const int WM_RBUTTONUP = 0x0205;

    private readonly LowLevelMouseProc _proc;
    private IntPtr _hookID = IntPtr.Zero;

    public MouseHook()
    {
        _proc = HookCallback;
    }

    public void Start()
    {
        if (_hookID == IntPtr.Zero)
        {
            _hookID = SetHook(_proc);
        }
    }

    public void Stop()
    {
        if (_hookID != IntPtr.Zero)
        {
            UnhookWindowsHookEx(_hookID);
            _hookID = IntPtr.Zero;
        }
    }

    private IntPtr SetHook(LowLevelMouseProc proc)
    {
        using var curProcess = Process.GetCurrentProcess();
        using var curModule = curProcess.MainModule;
        
        return SetWindowsHookEx(WH_MOUSE_LL, proc,
            GetModuleHandle(curModule?.ModuleName), 0);
    }

    private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

    private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0)
        {
            MSLLHOOKSTRUCT hookStruct = Marshal.PtrToStructure<MSLLHOOKSTRUCT>(lParam);
            var point = new Point(hookStruct.pt.x, hookStruct.pt.y);

            switch ((int)wParam)
            {
                case WM_MOUSEMOVE:
                    MouseMove?.Invoke(this, point);
                    break;
                case WM_LBUTTONDOWN:
                    LeftButtonDown?.Invoke(this, point);
                    break;
                case WM_LBUTTONUP:
                    LeftButtonUp?.Invoke(this, point);
                    break;
                case WM_RBUTTONDOWN:
                    RightButtonDown?.Invoke(this, point);
                    break;
                case WM_RBUTTONUP:
                    RightButtonUp?.Invoke(this, point);
                    break;
            }
        }

        return CallNextHookEx(_hookID, nCode, wParam, lParam);
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct POINT
    {
        public int x;
        public int y;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct MSLLHOOKSTRUCT
    {
        public POINT pt;
        public uint mouseData;
        public uint flags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string? lpModuleName);

    public void Dispose()
    {
        Stop();
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices; // �� DllImport ���ô� �����ռ�
using System.Reflection; // ʹ�� Assembly �����ô� �����ռ�
using System.Reflection.Emit; // ʹ�� ILGenerator ���ô� �����ռ�

class DLD
{

    // �������ݷ�ʽö�� ,ByValue ��ʾֵ���� ,ByRef ��ʾַ����
    public enum ModePass
    {
        ByValue = 0x0001,
        ByRef = 0x0002
    }
    ///����LoadLibrary��GetProcAddress��FreeLibrary��˽�б���hModule��farProc��
    /// <summary>
    /// ԭ���� :HMODULE LoadLibrary(LPCTSTR lpFileName);
    /// </summary>
    /// <param name="lpFileName">DLL �ļ��� </param>
    /// <returns> ������ģ��ľ�� </returns>
    [DllImport("kernel32.dll")]
    static extern IntPtr LoadLibrary(string lpFileName);
    /// <summary>
    /// ԭ���� : FARPROC GetProcAddress(HMODULE hModule, LPCWSTR lpProcName);
    /// </summary>
    /// <param name="hModule"> ��������ú����ĺ�����ģ��ľ�� </param>
    /// <param name="lpProcName"> ���ú��������� </param>
    /// <returns> ����ָ�� </returns>
    [DllImport("kernel32.dll")]
    static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);
    /// <summary>
    /// ԭ���� : BOOL FreeLibrary(HMODULE hModule);
    /// </summary>
    /// <param name="hModule"> ���ͷŵĺ�����ģ��ľ�� </param>
    /// <returns> �Ƿ����ͷ�ָ���� Dll</returns>
    [DllImport("kernel32", EntryPoint = "FreeLibrary", SetLastError = true)]
    static extern bool FreeLibrary(IntPtr hModule);
    /// <summary>
    /// Loadlibrary ���صĺ�����ģ��ľ��
    /// </summary>
    private IntPtr hModule = IntPtr.Zero;
    /// <summary>
    /// GetProcAddress ���صĺ���ָ��
    /// </summary>
    public IntPtr farProc = IntPtr.Zero;
    //���LoadDll��������Ϊ�˵���ʱ���㣬���������������
    /// <summary>
    /// װ�� Dll
    /// </summary>
    /// <param name="lpFileName">DLL �ļ��� </param>
    public void LoadDll(string lpFileName)
    {
        hModule = LoadLibrary(lpFileName);
        if (hModule == IntPtr.Zero)
            throw (new Exception(" û���ҵ� :" + lpFileName + "."));
    }
    //��������װ��Dll�ľ��������ʹ��LoadDll�����ĵڶ����汾��
    public void LoadDll(IntPtr HMODULE)
    {
        if (HMODULE == IntPtr.Zero)
            throw (new Exception(" ������ĺ�����ģ��ľ�� HMODULE Ϊ�� ."));
        hModule = HMODULE;
    }
    // ���LoadFun��������Ϊ�˵���ʱ���㣬Ҳ��������������������ľ�����뼰ע�����£�
    /// <summary>
    /// ��ú���ָ��
    /// </summary>
    /// <param name="lpProcName"> ���ú��������� </param>
    public void LoadFun(string lpProcName)
    {
        // ��������ģ��ľ��Ϊ�գ����׳��쳣
        if (hModule == IntPtr.Zero)
            throw (new Exception(" ������ģ��ľ��Ϊ�� , ��ȷ���ѽ��� LoadDll ���� !"));
        // ȡ�ú���ָ��
        farProc = GetProcAddress(hModule, lpProcName);
        // ������ָ�룬���׳��쳣
        if (farProc == IntPtr.Zero)
            throw (new Exception(" û���ҵ� :" + lpProcName + " �����������ڵ� "));
    }
    /// <summary>
    /// ��ú���ָ��
    /// </summary>
    /// <param name="lpFileName"> ��������ú����� DLL �ļ��� </param>
    /// <param name="lpProcName"> ���ú��������� </param>
    public void LoadFun(string lpFileName, string lpProcName)
    {// ȡ�ú�����ģ��ľ��
        hModule = LoadLibrary(lpFileName);
        // ��������ģ��ľ��Ϊ�գ����׳��쳣
        if (hModule == IntPtr.Zero)
            throw (new Exception(" û���ҵ� :" + lpFileName + "."));
        // ȡ�ú���ָ��
        farProc = GetProcAddress(hModule, lpProcName);
        // ������ָ�룬���׳��쳣
        if (farProc == IntPtr.Zero)
            throw (new Exception(" û���ҵ� :" + lpProcName + " �����������ڵ� "));
    }
    // ���UnLoadDll��Invoke������Invoke����Ҳ���������أ�
    /// <summary>
    /// ж�� Dll
    /// </summary>
    public void UnLoadDll()
    {
        FreeLibrary(hModule);
        hModule = IntPtr.Zero;
        farProc = IntPtr.Zero;
    }

    // <summary>
    /// �������趨�ĺ���
    /// </summary>
    /// <param name="ObjArray_Parameter"> ʵ�� </param>
    /// <param name="TypeArray_ParameterType"> ʵ������ </param>
    /// <param name="ModePassArray_Parameter"> ʵ�δ��ͷ�ʽ </param>
    /// <param name="Type_Return"> �������� </param>
    /// <returns> ���������ú����� object</returns>
    public object Invoke(object[] ObjArray_Parameter, Type[] TypeArray_ParameterType, ModePass[] ModePassArray_Parameter, Type Type_Return)
    {
        // ���� 3 �� if �ǽ��а�ȫ��� , ������ͨ�� , ���׳��쳣
        if (hModule == IntPtr.Zero)
            throw (new Exception(" ������ģ��ľ��Ϊ�� , ��ȷ���ѽ��� LoadDll ���� !"));
        if (farProc == IntPtr.Zero)
            throw (new Exception(" ����ָ��Ϊ�� , ��ȷ���ѽ��� LoadFun ���� !"));
        if (ObjArray_Parameter.Length != ModePassArray_Parameter.Length)
            throw (new Exception(" �����������䴫�ݷ�ʽ�ĸ�����ƥ�� ."));
        // �����Ǵ��� MyAssemblyName ���������� Name ����
        AssemblyName MyAssemblyName = new AssemblyName();
        MyAssemblyName.Name = "InvokeFun";
        // ���ɵ�ģ�����
        AssemblyBuilder MyAssemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(MyAssemblyName, AssemblyBuilderAccess.Run);
        ModuleBuilder MyModuleBuilder = MyAssemblyBuilder.DefineDynamicModule("InvokeDll");
        // ����Ҫ���õķ��� , ������Ϊ�� MyFun �������������ǡ� Type_Return �����������ǡ� TypeArray_ParameterType ��
        MethodBuilder MyMethodBuilder = MyModuleBuilder.DefineGlobalMethod("MyFun", MethodAttributes.Public | MethodAttributes.Static, Type_Return, TypeArray_ParameterType);
        // ��ȡһ�� ILGenerator �����ڷ�������� IL
        ILGenerator IL = MyMethodBuilder.GetILGenerator();
        int i;
        for (i = 0; i < ObjArray_Parameter.Length; i++)
        {// ��ѭ������������ѹ���ջ
            switch (ModePassArray_Parameter[i])
            {
                case ModePass.ByValue:
                    IL.Emit(OpCodes.Ldarg, i);
                    break;
                case ModePass.ByRef:
                    IL.Emit(OpCodes.Ldarga, i);
                    break;
                default:
                    throw (new Exception(" �� " + (i + 1).ToString() + " ������û�и�����ȷ�Ĵ��ݷ�ʽ ."));
            }
        }
        if (IntPtr.Size == 4)
        {// �жϴ���������
            IL.Emit(OpCodes.Ldc_I4, farProc.ToInt32());
        }
        else if (IntPtr.Size == 8)
        {
            IL.Emit(OpCodes.Ldc_I8, farProc.ToInt64());
        }
        else
        {
            throw new PlatformNotSupportedException();
        }
        IL.EmitCalli(OpCodes.Calli, CallingConvention.StdCall, Type_Return, TypeArray_ParameterType);
        IL.Emit(OpCodes.Ret); // ����ֵ
        MyModuleBuilder.CreateGlobalFunctions();
        // ȡ�÷�����Ϣ
        MethodInfo MyMethodInfo = MyModuleBuilder.GetMethod("MyFun");
        return MyMethodInfo.Invoke(null, ObjArray_Parameter);// ���÷�������������ֵ
    }
    /// <summary>
    /// �������趨�ĺ���
    /// </summary>
    /// <param name="IntPtr_Function"> ����ָ�� </param>
    /// <param name="ObjArray_Parameter"> ʵ�� </param>
    /// <param name="TypeArray_ParameterType"> ʵ������ </param>
    /// <param name="ModePassArray_Parameter"> ʵ�δ��ͷ�ʽ </param>
    /// <param name="Type_Return"> �������� </param>
    /// <returns> ���������ú����� object</returns>
    public object Invoke(IntPtr IntPtr_Function, object[] ObjArray_Parameter, Type[] TypeArray_ParameterType, ModePass[] ModePassArray_Parameter, Type Type_Return)
    {
        // ���� 2 �� if �ǽ��а�ȫ��� , ������ͨ�� , ���׳��쳣
        if (hModule == IntPtr.Zero)
            throw (new Exception(" ������ģ��ľ��Ϊ�� , ��ȷ���ѽ��� LoadDll ���� !"));
        if (IntPtr_Function == IntPtr.Zero)
            throw (new Exception(" ����ָ�� IntPtr_Function Ϊ�� !"));
        farProc = IntPtr_Function;
        return Invoke(ObjArray_Parameter, TypeArray_ParameterType, ModePassArray_Parameter, Type_Return);
    }

}


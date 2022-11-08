"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.genCode = void 0;
const CodeWriter_1 = require("./CodeWriter");
const SystemComponentType = ["GGraph", "GGroup", "GImage", "GList", "GLoader", "GLoader3D", "GMovieClip", "GRichTextField", "GTextField", "GTextInput"];
function genCode(handler) {
    const settings = handler.project.GetSettings("Publish").codeGeneration;
    //转换为中文拼音,删除特殊字符
    const codePkgName = handler.ToFilename(handler.pkg.name);
    //发布路径
    const exportCodePath = handler.exportCodePath + '/' + codePkgName;
    //发布的包名称
    const namespaceName = settings.packageName;
    //使用名称获取成员对象
    const getMemberByName = settings.getMemberByName;
    //需要代码生成的类
    const classes = handler.CollectClasses(settings.ignoreNoname, settings.ignoreNoname, null);
    //check if target folder exists, and delete old files
    handler.SetupCodeFolder(exportCodePath, "cs");
    const writer = new CodeWriter_1.default();
    for (let i = 0; i < classes.Count; i++) {
        const classInfo = classes.get_Item(i);
        const members = classInfo.members;
        const crossPackageFlagsArray = new Array();
        const customComponentFlagsArray = new Array();
        writer.reset();
        writer.writeln('using System.Threading.Tasks;');
        writer.writeln('using FairyGUI;');
        writer.writeln();
        writer.writeln('namespace %s.%s', namespaceName, codePkgName);
        writer.writeln('{');
        writer.writeln('    public sealed partial class %s : FObject', classInfo.className);
        writer.writeln('    {');
        writer.writeln('        public const string URL = "ui://%s%s";', handler.pkg.id, classInfo.resId);
        writer.writeln('        public const string UIResName = "%s";', classInfo.resName);
        writer.writeln('        public const string UIPackageName = "%s";', handler.pkg.name);
        writer.writeln();
        writer.writeln('        public override string ResName => UIResName;');
        writer.writeln();
        writer.writeln('        /// <summary>');
        writer.writeln('        /// %s的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。', classInfo.resName);
        writer.writeln('        /// </summary>');
        writer.writeln('        public %s self { get; private set; }', classInfo.superClassName);
        writer.writeln();
        for (let j = 0; j < members.Count; j++) {
            const memberInfo = members.get_Item(j);
            let typeName = memberInfo.type;
            crossPackageFlagsArray[j] = false;
            customComponentFlagsArray[j] = false;
            if (memberInfo.res != null) {
                customComponentFlagsArray[j] = true;
            }
            if (memberInfo.res != null && memberInfo.res.owner.name != handler.pkg.name) {
                crossPackageFlagsArray[j] = true;
                const resName = handler.ToFilename(memberInfo.res.name);
                const pkgName = handler.ToFilename(memberInfo.res.owner.name);
                typeName = pkgName + '.' + resName;
            }
            writer.writeln('        public %s %s;', typeName, memberInfo.varName);
        }
        writer.writeln();
        writer.writeln('        private static GObject CreateGObject()');
        writer.writeln('        {');
        writer.writeln('            return UIPackage.CreateObject(UIPackageName, UIResName);');
        writer.writeln('        }');
        writer.writeln();
        writer.writeln('        private static Task<GObject> CreateGObjectAsync()');
        writer.writeln('        {');
        writer.writeln('            var tcs = new TaskCompletionSource<GObject>();');
        writer.writeln('            UIPackage.CreateObjectAsync(UIPackageName, UIResName, (go) => tcs.SetResult(go));');
        writer.writeln('            return tcs.Task;');
        writer.writeln('        }');
        writer.writeln();
        writer.writeln('        internal static %s Create(GObject go)', classInfo.className);
        writer.writeln('        {');
        writer.writeln('            var fui = Fetch<%s>();', classInfo.className);
        writer.writeln('            fui.Init(go);');
        writer.writeln('            return fui;');
        writer.writeln('        }');
        writer.writeln();
        writer.writeln('        public static %s CreateInstance()', classInfo.className);
        writer.writeln('        {');
        writer.writeln('            return Create(CreateGObject());');
        writer.writeln('        }');
        writer.writeln();
        writer.writeln('        public static async Task<%s> CreateInstanceAsync()', classInfo.className);
        writer.writeln('        {');
        writer.writeln('            return Create(await CreateGObjectAsync());');
        writer.writeln('        }');
        writer.writeln();
        writer.writeln('        /// <summary>');
        writer.writeln('        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。');
        writer.writeln('        /// </summary>');
        writer.writeln('        public static %s GetFormPool(GObject go)', classInfo.className);
        writer.writeln('        {');
        writer.writeln('            var fui = go.GetFObject<%s>();', classInfo.className);
        writer.writeln('            if (fui == null)');
        writer.writeln('            {');
        writer.writeln('                fui = Create(go);');
        writer.writeln('                fui.IsFromPool = true;');
        writer.writeln('            }');
        writer.writeln('            return fui;');
        writer.writeln('        }');
        writer.writeln();
        writer.writeln('        private void Init(GObject go)');
        writer.writeln('        {');
        writer.writeln('            if (go == null)');
        writer.writeln('            {');
        writer.writeln('                return;');
        writer.writeln('            }');
        writer.writeln();
        writer.writeln('            GObject = go;');
        writer.writeln('            IsDisposed = false;');
        writer.writeln();
        writer.writeln('            self = (%s)go;', classInfo.superClassName);
        writer.writeln();
        writer.writeln('            self.AddFObject(this);');
        writer.writeln();
        writer.writeln('            if (string.IsNullOrWhiteSpace(Name))');
        writer.writeln('            {');
        writer.writeln('                Name = UIResName;');
        writer.writeln('            }');
        writer.writeln();
        writer.writeln('            var com = go.asCom;');
        writer.writeln();
        writer.writeln('            if (com != null)');
        writer.writeln('            {');
        for (let j = 0; j < members.Count; j++) {
            const memberInfo = members.get_Item(j);
            switch (memberInfo.group) {
                // 成员
                case 0:
                    if (getMemberByName) {
                        if (crossPackageFlagsArray[j]) {
                            const resName = handler.ToFilename(memberInfo.res.name);
                            const pkgName = handler.ToFilename(memberInfo.res.owner.name);
                            writer.writeln('                %s = %s.Create(com.GetChild("%s"));', memberInfo.name, pkgName + '.' + resName, memberInfo.name);
                        }
                        else {
                            if (customComponentFlagsArray[j])
                                writer.writeln('                %s = %s.Create(com.GetChild("%s"));', memberInfo.name, memberInfo.type, memberInfo.name);
                            else
                                writer.writeln('                %s = (%s)com.GetChild("%s");', memberInfo.name, memberInfo.type, memberInfo.name);
                        }
                    }
                    else {
                        if (crossPackageFlagsArray[j]) {
                            const resName = handler.ToFilename(memberInfo.res.name);
                            const pkgName = handler.ToFilename(memberInfo.res.owner.name);
                            writer.writeln('                %s = %s.Create(com.GetChildAt(%s));', memberInfo.name, pkgName + '.' + resName, memberInfo.index);
                        }
                        else {
                            if (customComponentFlagsArray[j])
                                writer.writeln('                %s = %s.Create(com.GetChildAt(%s));', memberInfo.name, memberInfo.type, memberInfo.index);
                            else
                                writer.writeln('                %s = (%s)com.GetChildAt(%s);', memberInfo.name, memberInfo.type, memberInfo.index);
                        }
                    }
                    break;
                // 控制器
                case 1:
                    if (getMemberByName)
                        writer.writeln('                %s = com.GetController("%s");', memberInfo.varName, memberInfo.name);
                    else
                        writer.writeln('                %s = com.GetControllerAt(%s);', memberInfo.varName, memberInfo.index);
                    break;
                // 动效
                case 2:
                    if (getMemberByName)
                        writer.writeln('                %s = com.GetTransition("%s");', memberInfo.varName, memberInfo.name);
                    else
                        writer.writeln('                %s = com.GetTransitionAt(%s);', memberInfo.varName, memberInfo.index);
                    break;
            }
        }
        writer.writeln('            }');
        writer.writeln('        }');
        writer.writeln();
        writer.writeln('        public override void Dispose()');
        writer.writeln('        {');
        writer.writeln('            if (IsDisposed)');
        writer.writeln('            {');
        writer.writeln('                return;');
        writer.writeln('            }');
        writer.writeln();
        writer.writeln('            self.RemoveFObject();');
        writer.writeln('            self = null;');
        for (let j = 0; j < members.Count; j++) {
            const memberInfo = members.get_Item(j);
            if (memberInfo.group === 0) {
                if (customComponentFlagsArray[j]) {
                    writer.writeln('            %s.Dispose();', memberInfo.varName);
                }
                writer.writeln('            %s = null;', memberInfo.varName);
            }
            else {
                writer.writeln('            %s = null;', memberInfo.varName);
            }
        }
        writer.writeln();
        writer.writeln('            base.Dispose();');
        writer.writeln('        }');
        writer.writeln('    }');
        writer.writeln('}');
        writer.save(exportCodePath + '/' + classInfo.className + '.cs');
    }
    const binderName = 'Package' + codePkgName;
    writer.reset();
    writer.writeln('namespace %s', namespaceName);
    writer.startBlock();
    writer.writeln('public static partial class FUIPackage');
    writer.startBlock();
    writer.writeln('public const string %s = "%s";', codePkgName, handler.pkg.name);
    for (let i = 0; i < handler.items.Count; i++) {
        const packageItem = handler.items.get_Item(i);
        const tempCodeName = packageItem.name.replace(/-/g, "_");
        if (packageItem.branch.length === 0) {
            writer.writeln('public const string %s_%s = "ui://%s/%s";', codePkgName, tempCodeName, handler.pkg.name, packageItem.name);
        }
    }
    writer.endBlock(); //class
    writer.endBlock(); //namespace
    writer.save(exportCodePath + '/' + binderName + '.cs');
}
exports.genCode = genCode;

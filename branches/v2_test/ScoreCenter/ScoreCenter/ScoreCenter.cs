﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.4206
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=2.0.50727.1432.
// 
namespace MediaPortal.Plugin.ScoreCenter {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public partial class Style {
        
        private string nameField;
        
        private long foreColorField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public long ForeColor {
            get {
                return this.foreColorField;
            }
            set {
                this.foreColorField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public partial class Rule {
        
        private int columnField;
        
        private Operation operatorField;
        
        private string valueField;
        
        private RuleAction actionField;
        
        private string formatField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Column {
            get {
                return this.columnField;
            }
            set {
                this.columnField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public Operation Operator {
            get {
                return this.operatorField;
            }
            set {
                this.operatorField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Value {
            get {
                return this.valueField;
            }
            set {
                this.valueField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public RuleAction Action {
            get {
                return this.actionField;
            }
            set {
                this.actionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Format {
            get {
                return this.formatField;
            }
            set {
                this.formatField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    public enum Operation {
        
        /// <remarks/>
        EqualTo,
        
        /// <remarks/>
        NotEqualTo,
        
        /// <remarks/>
        GT,
        
        /// <remarks/>
        LT,
        
        /// <remarks/>
        GE,
        
        /// <remarks/>
        LE,
        
        /// <remarks/>
        Contains,
        
        /// <remarks/>
        NotContains,
        
        /// <remarks/>
        StartsWith,
        
        /// <remarks/>
        NotStartsWith,
        
        /// <remarks/>
        EndsWith,
        
        /// <remarks/>
        NotEndsWith,
        
        /// <remarks/>
        MOD,
        
        /// <remarks/>
        InList,
        
        /// <remarks/>
        IsNull,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    public enum RuleAction {
        
        /// <remarks/>
        FormatCell,
        
        /// <remarks/>
        FormatLine,
        
        /// <remarks/>
        MergeCells,
        
        /// <remarks/>
        ReplaceText,
        
        /// <remarks/>
        SkipLine,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public partial class Score {
        
        private string nameField;
        
        private string urlField;
        
        private string encodingField;
        
        private string xPathField;
        
        private string elementField;
        
        private BetweenElements betweenEltsField;
        
        private string headersField;
        
        private string sizesField;
        
        private int skipField;
        
        private int maxLinesField;
        
        private string imageField;
        
        private string parseOptionsField;
        
        private int variableField;
        
        private Rule[] rulesField;
        
        private string idField;
        
        private Node typeField;
        
        private string parentField;
        
        private int orderField;
        
        private bool enableField;
        
        public Score() {
            this.encodingField = "";
            this.elementField = "";
            this.betweenEltsField = BetweenElements.None;
            this.headersField = "";
            this.sizesField = "";
            this.skipField = 0;
            this.maxLinesField = 0;
            this.imageField = "";
            this.parseOptionsField = "None";
            this.variableField = 0;
            this.typeField = Node.Score;
            this.parentField = "";
            this.orderField = 99;
            this.enableField = true;
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Url {
            get {
                return this.urlField;
            }
            set {
                this.urlField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string Encoding {
            get {
                return this.encodingField;
            }
            set {
                this.encodingField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string XPath {
            get {
                return this.xPathField;
            }
            set {
                this.xPathField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string Element {
            get {
                return this.elementField;
            }
            set {
                this.elementField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.ComponentModel.DefaultValueAttribute(BetweenElements.None)]
        public BetweenElements BetweenElts {
            get {
                return this.betweenEltsField;
            }
            set {
                this.betweenEltsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string Headers {
            get {
                return this.headersField;
            }
            set {
                this.headersField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string Sizes {
            get {
                return this.sizesField;
            }
            set {
                this.sizesField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.ComponentModel.DefaultValueAttribute(0)]
        public int Skip {
            get {
                return this.skipField;
            }
            set {
                this.skipField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.ComponentModel.DefaultValueAttribute(0)]
        public int MaxLines {
            get {
                return this.maxLinesField;
            }
            set {
                this.maxLinesField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string Image {
            get {
                return this.imageField;
            }
            set {
                this.imageField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.ComponentModel.DefaultValueAttribute("None")]
        public string ParseOptions {
            get {
                return this.parseOptionsField;
            }
            set {
                this.parseOptionsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.ComponentModel.DefaultValueAttribute(0)]
        public int variable {
            get {
                return this.variableField;
            }
            set {
                this.variableField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Rule", IsNullable=false)]
        public Rule[] Rules {
            get {
                return this.rulesField;
            }
            set {
                this.rulesField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Id {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(Node.Score)]
        public Node Type {
            get {
                return this.typeField;
            }
            set {
                this.typeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string Parent {
            get {
                return this.parentField;
            }
            set {
                this.parentField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(99)]
        public int Order {
            get {
                return this.orderField;
            }
            set {
                this.orderField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(true)]
        public bool enable {
            get {
                return this.enableField;
            }
            set {
                this.enableField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    public enum BetweenElements {
        
        /// <remarks/>
        None,
        
        /// <remarks/>
        EmptyLine,
        
        /// <remarks/>
        RepeatHeader,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    public enum Node {
        
        /// <remarks/>
        Score,
        
        /// <remarks/>
        Folder,
        
        /// <remarks/>
        RSS,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public partial class ScoreCenter {
        
        private ScoreCenterSetup setupField;
        
        private Style[] stylesField;
        
        private Score[] scoresField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ScoreCenterSetup Setup {
            get {
                return this.setupField;
            }
            set {
                this.setupField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Style", IsNullable=false)]
        public Style[] Styles {
            get {
                return this.stylesField;
            }
            set {
                this.stylesField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Score", IsNullable=false)]
        public Score[] Scores {
            get {
                return this.scoresField;
            }
            set {
                this.scoresField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class ScoreCenterSetup {
        
        private string nameField;
        
        private int cacheExpirationField;
        
        private int defaultSkinColorField;
        
        private int defaultFontColorField;
        
        private string backdropDirField;
        
        private UpdateMode updateOnlineModeField;
        
        private string updateUrlField;
        
        private string updateRuleField;
        
        private string homeField;
        
        private int versionField;
        
        public ScoreCenterSetup() {
            this.cacheExpirationField = 5;
            this.defaultSkinColorField = -16776961;
            this.defaultFontColorField = -1;
            this.backdropDirField = "";
            this.updateOnlineModeField = UpdateMode.Never;
            this.updateUrlField = "";
            this.updateRuleField = "";
            this.homeField = "";
            this.versionField = 1;
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.ComponentModel.DefaultValueAttribute(5)]
        public int CacheExpiration {
            get {
                return this.cacheExpirationField;
            }
            set {
                this.cacheExpirationField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.ComponentModel.DefaultValueAttribute(-16776961)]
        public int DefaultSkinColor {
            get {
                return this.defaultSkinColorField;
            }
            set {
                this.defaultSkinColorField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.ComponentModel.DefaultValueAttribute(-1)]
        public int DefaultFontColor {
            get {
                return this.defaultFontColorField;
            }
            set {
                this.defaultFontColorField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string BackdropDir {
            get {
                return this.backdropDirField;
            }
            set {
                this.backdropDirField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.ComponentModel.DefaultValueAttribute(UpdateMode.Never)]
        public UpdateMode UpdateOnlineMode {
            get {
                return this.updateOnlineModeField;
            }
            set {
                this.updateOnlineModeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string UpdateUrl {
            get {
                return this.updateUrlField;
            }
            set {
                this.updateUrlField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string UpdateRule {
            get {
                return this.updateRuleField;
            }
            set {
                this.updateRuleField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string Home {
            get {
                return this.homeField;
            }
            set {
                this.homeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(1)]
        public int version {
            get {
                return this.versionField;
            }
            set {
                this.versionField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    public enum UpdateMode {
        
        /// <remarks/>
        Never,
        
        /// <remarks/>
        Once,
        
        /// <remarks/>
        Always,
        
        /// <remarks/>
        Manually,
    }
}
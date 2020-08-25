using System;
using System.Text;
using System.Linq;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using System.Collections.Generic;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.DomainLogics;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.Xpo;
using InsERTSubiektNexo.XAF.Module.Services;
using InsERTSubiektNexo.XAF.Module.BusinessObjects;

namespace InsERTSubiektNexo.XAF.Module {
    // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ModuleBase.
    public sealed partial class XAFModule : ModuleBase {
        private readonly SerwisAsortymentow _srvAsortymentow;
        private readonly SerwisPodmiotow _srvPodmiotow;
        Dictionary<int, Asortyment> asortymentyCache;
        public XAFModule() {
            _srvAsortymentow = new SerwisAsortymentow();
            _srvPodmiotow = new SerwisPodmiotow();
            asortymentyCache = new Dictionary<int, Asortyment>();
            var tmp  =  _srvAsortymentow.PodajWszystkieAsortymenty();
            for (int i = 0; i < tmp.Count; i++)
            {
                asortymentyCache.Add(i, tmp[i]);
            }
            InitializeComponent();
			BaseObject.OidInitializationMode = OidInitializationMode.AfterConstruction;
        }
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB) {
            ModuleUpdater updater = new DatabaseUpdate.Updater(objectSpace, versionFromDB);
            return new ModuleUpdater[] { updater };
        }

        public override void Setup(XafApplication application) {
            base.Setup(application);
            // Manage various aspects of the application UI and behavior at the module level.
            application.SetupComplete += Application_SetupComplete;
        }
        private void Application_SetupComplete(object sender, EventArgs e)
        {
            Application.ObjectSpaceCreated += Application_ObjectSpaceCreated;
        }
        private void Application_ObjectSpaceCreated(object sender, ObjectSpaceCreatedEventArgs e)
        {
            var nonPersistentObjectSpace = (NonPersistentObjectSpace)e.ObjectSpace;
            if (nonPersistentObjectSpace != null)
            {
                nonPersistentObjectSpace.ObjectsGetting += ObjectSpace_ObjectsGetting;
                nonPersistentObjectSpace.ObjectGetting += NonPersistentObjectSpace_ObjectGetting;
                nonPersistentObjectSpace.Committing += NonPersistentObjectSpace_Committing;
            }
        }
        private void NonPersistentObjectSpace_ObjectGetting(object sender, ObjectGettingEventArgs e)
        {
            if (e.SourceObject is Asortyment)
            {
                ((IObjectSpaceLink)e.TargetObject).ObjectSpace = (IObjectSpace)sender;
            }
        }
        private void NonPersistentObjectSpace_Committing(Object sender, CancelEventArgs e)
        {
            IObjectSpace objectSpace = (IObjectSpace)sender;
            foreach (Object obj in objectSpace.ModifiedObjects)
            {
                // możliwość dodawania i kasowania encji powinna być wyłączona z UI, jakoś trzeba wyłączyć górną belkę.
                Asortyment commitedObj = obj as Asortyment;
                if (commitedObj != null)
                {
                    _srvAsortymentow.ZmienOpis(commitedObj.Symbol, commitedObj.Opis);
                }
            }
        }
        private void ObjectSpace_ObjectsGetting(Object sender, ObjectsGettingEventArgs e)
        {
            var objectSpace = (IObjectSpace)sender;
            var asortymenty = new BindingList<Asortyment>();
            if (e.ObjectType == typeof(BusinessObjects.Asortyment))
            {
                asortymenty.AllowNew = false;
                asortymenty.AllowEdit = true;
                asortymenty.AllowRemove = false;
                foreach (var item in asortymentyCache.Values)
                {
                    asortymenty.Add(objectSpace.GetObject<Asortyment>(item));
                }
                e.Objects = asortymenty;
            }
        }
        public override void CustomizeTypesInfo(ITypesInfo typesInfo) {
            base.CustomizeTypesInfo(typesInfo);
            CalculatedPersistentAliasHelper.CustomizeTypesInfo(typesInfo);
        }
    }
}

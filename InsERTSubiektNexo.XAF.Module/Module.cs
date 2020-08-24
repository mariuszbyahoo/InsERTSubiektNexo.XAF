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
        public XAFModule() {
            InitializeComponent();
			BaseObject.OidInitializationMode = OidInitializationMode.AfterConstruction;
            _srvAsortymentow = new SerwisAsortymentow();
            _srvPodmiotow = new SerwisPodmiotow();
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
            }
        }
        private void ObjectSpace_ObjectsGetting(Object sender, ObjectsGettingEventArgs e)
        {
            if (e.ObjectType == typeof(BusinessObjects.Asortyment))
            {
                BindingList<BusinessObjects.Asortyment> asortymenty = _srvAsortymentow.PodajWszystkieAsortymenty();

                e.Objects = asortymenty;
            }
        }
        public override void CustomizeTypesInfo(ITypesInfo typesInfo) {
            base.CustomizeTypesInfo(typesInfo);
            CalculatedPersistentAliasHelper.CustomizeTypesInfo(typesInfo);
        }
    }
}

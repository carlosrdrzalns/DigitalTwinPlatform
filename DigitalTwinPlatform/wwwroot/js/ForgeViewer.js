
var viewer;
var view;
var doc;
var ModelExtension;
var ModelName;
var Username;
var Version;
var modelos = []
var multilinkNodes = [];
var modelosSiguientes = [];
var modelosACargar = [];
var versionesACargar = [];
var urnsACargar = [];
var planos = {};
var vistas3D = {};
var hideOld = false;
var contador = 0;
var issueEventListener = false;
var originalState;

// @urn the model to show
// @viewablesId which viewables to show, applies to BIM 360 Plans folder


async function launchViewer(urn, modelname, username, viewableId) {
    Username = username;
    ModelName = modelname;
    $('.nav-model-title').html(modelname);
    ModelExtension = modelname.substring(modelname.lastIndexOf(".") + 1);
    var userLanguage = navigator.language || navigator.userLanguage;
    var options = {
        language: userLanguage,
        env: 'AutodeskProduction',
        getAccessToken: getForgeToken,
        api: 'derivativeV2' + (atob(urn.replace('_', '/')).indexOf('emea') > -1 ? '_EU' : '') // handle BIM 360 US and EU regions
    };
 
    Autodesk.Viewing.theExtensionManager.unregisterExtension('Autodesk.Explode');
    Autodesk.Viewing.Initializer(options, () => {

        viewer = new Autodesk.Viewing.GuiViewer3D(document.getElementById('forgeViewer'), {
            extensions: ["Autodesk.AEC.LevelsExtension", "Autodesk.AEC.Minimap3DExtension", "Autodesk.DataVisualization"],
            loaderExtensions: { svf: "Autodesk.MemoryLimited" },
            memory: {
                limit: 1024
            }
        });
        viewer.params = {
            ModelName: modelname,
            ModelExtension: ModelExtension,
            Username: username,

        };
        viewer.start();
        viewer.hideLines(true);
        var documentId = 'urn:' + urn;
        Autodesk.Viewing.Document.load(documentId, onDocumentLoadSuccess, onDocumentLoadFailure);
    });

    async function onDocumentLoadSuccess(docum) {
        // if a viewableId was specified, load that view, otherwise the default view
        
        doc = docum;
        var viewables = (viewableId ? doc.getRoot().findByGuid(viewableId) : doc.getRoot().getDefaultGeometry());
        doc.downloadAecModelData();
        viewer.loadDocumentNode(doc, viewables).then(async function (i) {
            originalState = viewer.getState();
            //viewer.addEventListener(Autodesk.Viewing.GEOMETRY_LOADED_EVENT, (x) => {
            //    let explodeExtension = viewer.getExtension('Autodesk.Explode');
            //    explodeExtension.unload();
            //}, { once: true });
            // any additional action here?
            //document.addEventListener('wheel', function (event) {
            //    let viewerpos = viewer.container.getBoundingClientRect();
            //    let screenX = event.clientX - viewerpos.x;
            //    let screenY = event.clientY - viewerpos.y;
            //    let pos = viewer.clientToWorld(screenX, screenY);
            //    if (pos != null) {
            //        viewer.navigation.setPivotPoint(pos)
            //    }

            //});
        });
    }

    function onDocumentLoadFailure(viewerErrorCode) {
        console.error('onDocumentLoadFailure() - errorCode:' + viewerErrorCode);
    }


}


function getForgeToken(callback) {
  fetch('/APS/getToken').then(res => {
    res.json().then(data => {
      callback(data.access_token, data.expires_in);
    });
  });
}



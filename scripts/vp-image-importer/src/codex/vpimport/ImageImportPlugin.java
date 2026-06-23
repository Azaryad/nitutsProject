package codex.vpimport;

import com.vp.plugin.ApplicationManager;
import com.vp.plugin.DiagramManager;
import com.vp.plugin.ProjectManager;
import com.vp.plugin.VPPlugin;
import com.vp.plugin.VPPluginCommandLineSupport;
import com.vp.plugin.VPPluginInfo;
import com.vp.plugin.diagram.IDiagramUIModel;
import com.vp.plugin.diagram.shape.IImageShapeUIModel;
import com.vp.plugin.model.factory.IModelElementFactory;

import javax.imageio.ImageIO;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;
import java.util.Arrays;
import java.util.HashMap;
import java.util.Map;

public class ImageImportPlugin implements VPPlugin, VPPluginCommandLineSupport {
    private static final String[] ORDER = {
        "obligation_tiers",
        "layer_dependency_overview",
        "domain_model",
        "external_services_layer",
        "ui_infrastructure_layer",
        "flow_a_view_open_trips",
        "flow_b_assign_trip_to_region",
        "flow_c_send_trip_offer",
        "flow_d_respond_to_trip_offer",
        "flow_e_forward_offer_to_next_driver",
        "flow_f_update_ride_control",
        "appendix_startup_data_load"
    };

    @Override
    public void loaded(VPPluginInfo info) {
    }

    @Override
    public void unloaded() {
    }

    @Override
    public void invoke(String[] args) {
        Map<String, String> parsed = parseArgs(args);
        String imageDirPath = parsed.get("-images");
        if (imageDirPath == null || imageDirPath.trim().isEmpty()) {
            System.out.println("Usage: -images <png_folder>");
            return;
        }

        File imageDir = new File(imageDirPath);
        File[] images = imageDir.listFiles((dir, name) -> name.toLowerCase().endsWith(".png"));
        if (images == null || images.length == 0) {
            System.out.println("No PNG files found in " + imageDir.getAbsolutePath());
            return;
        }

        Map<String, Integer> order = new HashMap<>();
        for (int i = 0; i < ORDER.length; i++) {
            order.put(ORDER[i], i);
        }
        Arrays.sort(images, (a, b) -> {
            int ai = order.getOrDefault(baseName(a), Integer.MAX_VALUE);
            int bi = order.getOrDefault(baseName(b), Integer.MAX_VALUE);
            if (ai != bi) {
                return Integer.compare(ai, bi);
            }
            return a.getName().compareToIgnoreCase(b.getName());
        });

        DiagramManager diagramManager = ApplicationManager.instance().getDiagramManager();
        int count = 0;
        for (File image : images) {
            try {
                createImageDiagram(diagramManager, image);
                count++;
                System.out.println("Imported " + image.getName());
            } catch (IOException e) {
                System.out.println("Failed to import " + image.getAbsolutePath() + ": " + e.getMessage());
            }
        }

        ProjectManager projectManager = ApplicationManager.instance().getProjectManager();
        projectManager.saveProject();
        System.out.println("Imported " + count + " image diagrams and saved project.");
    }

    private void createImageDiagram(DiagramManager diagramManager, File imageFile) throws IOException {
        BufferedImage image = ImageIO.read(imageFile);
        if (image == null) {
            throw new IOException("Unsupported image file");
        }

        IDiagramUIModel diagram = diagramManager.createDiagram(DiagramManager.DIAGRAM_TYPE_OVERVIEW_DIAGRAM);
        diagram.setName(prettyName(baseName(imageFile)));
        diagram.setDocumentation("Rendered from docs/design/implementation-uml.puml and imported as a visual PNG.");

        IImageShapeUIModel imageShape = (IImageShapeUIModel) diagramManager.createDiagramElement(
            diagram,
            IModelElementFactory.instance().createImageShape()
        );
        imageShape.setImagePath(imageFile.getAbsolutePath());
        imageShape.setMode(IImageShapeUIModel.LINKED);
        imageShape.setScaling(IImageShapeUIModel.ACTUAL_SIZE);
        imageShape.setImage(image);
        imageShape.setBounds(40, 40, image.getWidth(), image.getHeight());
        diagram.setBounds(0, 0, image.getWidth() + 80, image.getHeight() + 80);
    }

    private Map<String, String> parseArgs(String[] args) {
        Map<String, String> parsed = new HashMap<>();
        if (args == null) {
            return parsed;
        }

        for (int i = 0; i < args.length; i++) {
            if (args[i].startsWith("-") && i + 1 < args.length && !args[i + 1].startsWith("-")) {
                parsed.put(args[i], args[++i]);
            }
        }
        return parsed;
    }

    private static String baseName(File file) {
        String name = file.getName();
        int dot = name.lastIndexOf('.');
        return dot >= 0 ? name.substring(0, dot) : name;
    }

    private static String prettyName(String baseName) {
        switch (baseName) {
            case "obligation_tiers":
                return "Implementation UML - Obligation Tiers";
            case "layer_dependency_overview":
                return "Implementation UML - Layer Dependency Overview";
            case "domain_model":
                return "Implementation UML - Domain Model";
            case "external_services_layer":
                return "Implementation UML - External Services Layer";
            case "ui_infrastructure_layer":
                return "Implementation UML - UI Infrastructure Layer";
            case "flow_a_view_open_trips":
                return "Implementation UML - Flow A View Open Trips";
            case "flow_b_assign_trip_to_region":
                return "Implementation UML - Flow B Assign Trip to Region";
            case "flow_c_send_trip_offer":
                return "Implementation UML - Flow C Send Trip Offer";
            case "flow_d_respond_to_trip_offer":
                return "Implementation UML - Flow D Respond to Trip Offer";
            case "flow_e_forward_offer_to_next_driver":
                return "Implementation UML - Flow E Forward Offer";
            case "flow_f_update_ride_control":
                return "Implementation UML - Flow F Update Ride Control";
            case "appendix_startup_data_load":
                return "Implementation UML - Appendix Startup Data Load";
            case "state_trip":
                return "Trip - State Machine";
            default:
                return "Implementation UML - " + baseName.replace('_', ' ');
        }
    }
}

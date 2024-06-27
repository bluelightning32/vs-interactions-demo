using Vintagestory.API.Common;

namespace Interactions.Items;

public class LogInteractions : Item {

  private bool _startInteractionClient = true;
  private bool _startInteractionServer = true;
  private bool _cancelInteractionClient = true;
  private bool _cancelInteractionServer = true;
  private int _stepCount = 0;
  private double _interactionDurationClient = 1;
  private double _interactionDurationServer = 1;

  public override void OnLoaded(ICoreAPI api) {
    base.OnLoaded(api);

    _startInteractionClient = Attributes["startInteractionClient"].AsBool(true);
    _startInteractionServer = Attributes["startInteractionServer"].AsBool(true);
    _cancelInteractionClient =
        Attributes["cancelInteractionClient"].AsBool(true);
    _cancelInteractionServer =
        Attributes["cancelInteractionServer"].AsBool(true);
    _interactionDurationClient =
        Attributes["interactionDurationClient"].AsDouble(1);
    _interactionDurationServer =
        Attributes["interactionDurationServer"].AsDouble(1);
    HeldPriorityInteract = Attributes["priority"].AsBool(false);
  }

  public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handling) {
    api.Logger.Notification("OnHeldInteractStart code={0}, side={1}, firstEvent={2}", Code,
                            api.Side, firstEvent);
    _stepCount = 0;
    if (api.Side == EnumAppSide.Client ? _startInteractionClient
                                          : _startInteractionServer) {
                                            handling = EnumHandHandling.Handled;
                                          }
  }

  public override bool OnHeldInteractStep(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel) {
 api.Logger.Notification(
        "OnHeldInteractStep code={0}, side={1}, count={2}, secondsUsed={3}",
        Code, api.Side, _stepCount++, secondsUsed);
    // Return true to continue the interaction.
    return secondsUsed < (api.Side == EnumAppSide.Client
                              ? _interactionDurationClient
                              : _interactionDurationServer);
  }

  public override bool OnHeldInteractCancel(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, EnumItemUseCancelReason cancelReason) {
    api.Logger.Notification("OnHeldInteractCancel code={0}, side={1}, " +
                            "cancelReason={2}",
                            Code, api.Side, cancelReason);
    // Return true to allow the interaction to stop.
    return api.Side == EnumAppSide.Client ? _cancelInteractionClient
                                          : _cancelInteractionServer;
  }

  public override void OnHeldInteractStop(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel) {
    api.Logger.Notification(
        "OnHeldInteractStop code={0}, side={1}", Code,
        api.Side);
  }
}

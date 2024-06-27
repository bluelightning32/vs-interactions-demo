using Vintagestory.API.Common;

namespace Interactions.Blocks;

public class LogInteractions : Block {

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
    PlacedPriorityInteract = Attributes["priority"].AsBool(false);
  }
  public override bool OnBlockInteractStart(IWorldAccessor world,
                                            IPlayer byPlayer,
                                            BlockSelection blockSel) {
    api.Logger.Notification("OnBlockInteractStart code={0}, side={1}", Code,
                            api.Side);
    _stepCount = 0;
    return api.Side == EnumAppSide.Client ? _startInteractionClient
                                          : _startInteractionServer;
  }

  public override bool OnBlockInteractStep(float secondsUsed,
                                           IWorldAccessor world,
                                           IPlayer byPlayer,
                                           BlockSelection blockSel) {
    api.Logger.Notification(
        "OnBlockInteractStep code={0}, side={1}, count={2}, secondsUsed={3}",
        Code, api.Side, _stepCount++, secondsUsed);
    // Return true to continue the interaction.
    return secondsUsed < (api.Side == EnumAppSide.Client
                              ? _interactionDurationClient
                              : _interactionDurationServer);
  }

  public override bool
  OnBlockInteractCancel(float secondsUsed, IWorldAccessor world,
                        IPlayer byPlayer, BlockSelection blockSel,
                        EnumItemUseCancelReason cancelReason) {
    api.Logger.Notification("OnBlockInteractCancel code={0}, side={1}, " +
                            "secondsUsed={2}, cancelReason={3}",
                            Code, api.Side, secondsUsed, cancelReason);
    // Return true to allow the interaction to stop.
    return api.Side == EnumAppSide.Client ? _cancelInteractionClient
                                          : _cancelInteractionServer;
  }

  public override void OnBlockInteractStop(float secondsUsed,
                                           IWorldAccessor world,
                                           IPlayer byPlayer,
                                           BlockSelection blockSel) {
    api.Logger.Notification(
        "OnBlockInteractStop code={0}, side={1}, secondsUsed={2}", Code,
        api.Side, secondsUsed);
  }
}

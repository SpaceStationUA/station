using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.Pulling.Components
{
    [RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
    [Access(typeof(SharedPullingStateManagementSystem))]
    public sealed partial class SharedPullerComponent : Component
    {
        // Before changing how this is updated, please see SharedPullerSystem.RefreshMovementSpeed
        public float WalkSpeedModifier => Pulling == default ? 1.0f : 0.95f;

        public float SprintSpeedModifier => Pulling == default ? 1.0f : 0.95f;

        [DataField, AutoNetworkedField]
        public EntityUid? Pulling { get; set; }

        /// <summary>
        ///     Does this entity need hands to be able to pull something?
        /// </summary>
        [DataField("needsHands")]
        public bool NeedsHands = true;

        // My raiding guild
        /// <summary>
        /// Next time the puller can throw what is being pulled.
        /// Used to avoid spamming it for infinite spin + velocity.
        /// </summary>
        [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), AutoNetworkedField]
        public TimeSpan NextThrow;

        [DataField]
        public TimeSpan ThrowCooldown = TimeSpan.FromSeconds(1);
    }
}

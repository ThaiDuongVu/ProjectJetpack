public enum EnemyState
{
    // Enemy hang around without doing much
    Idle,

    // Enemy follows player around game world
    Follow,

    // Enemy tries to attack player
    Attack,

    // Enemy staggered by player attacked
    Stagger
}
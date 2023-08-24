using forDino;
using UnityEngine;

namespace Interface
{
    public interface IDinoState
    {
        // Unity event 함수가 아닌 것에 유의.
        void Start(IStateHandler dino);
        void Update(IStateHandler dino);
        void OnCollisionEnter(Collision2D collision, IStateHandler dino);
    }
}
using Entities.Targets.Bosses;
using UnityEditor;

namespace Editor
{
    [CustomEditor(typeof(BossTarget), true)]
    public class BossTargetEditor : UnityEditor.Editor
    {
        private const float MinError = 0.01f;
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var stages = ((BossTarget)target).stages;

            for (var i = stages.Count - 2; i >= 0; i--)
            {
                if (stages[i].startHealthPercent - stages[i + 1].startHealthPercent < MinError)
                {
                    stages[i].startHealthPercent = stages[i + 1].startHealthPercent + MinError;
                }
                
                if (stages[i].startHealthPercent > 1 - MinError * i)
                {
                    stages[i].startHealthPercent = 1 - MinError * i;
                }
            }
            
            if (stages[^1].startHealthPercent > 1 - MinError * (stages.Count - 1))
            {
                stages[^1].startHealthPercent = 1 - MinError * (stages.Count - 1);
            }
        }
    }
}
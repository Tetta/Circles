using System.Collections;
using UnityEngine;

namespace GameBench
{
    public class ChunkSlice : MonoBehaviour
    {
        public SpriteRenderer iconSpRend, spRend;
        public RewardType rewardType;
        public TextMesh valueText;
        public PointCollider[] pointCollider;
        int myIndex;
        void Start()
        {
            myIndex = transform.GetSiblingIndex();
            rewardType = SpinWheelSetup.Instance.rewarItem[myIndex].rewardType;
            iconSpRend.sprite = SpinWheelSetup.Instance.rewarItem[myIndex].rewardSprite;
            valueText.text = SpinWheelSetup.Instance.rewarItem[myIndex].rewardQuantity.ToString();
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            
            //spRend.sprite = SpinWheel.Instance.chunkSp[1];

            //Debug.Log(name);
            //Debug.Log(other.name);
            if (other.name == "Arrow") {
                spRend.transform.Find("ActiveChunck").gameObject.SetActive(true);
                SpinWheel.Instance.SelectedReward = transform.GetSiblingIndex();
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.name == "Arrow") {
                //spRend.sprite = SpinWheel.Instance.chunkSp[0];
                spRend.transform.Find("ActiveChunck").gameObject.SetActive(false);
            }

        }
        public IEnumerator animateActiveChunck() {
            yield return new WaitForSecondsRealtime(0.2f);
            spRend.transform.Find("ActiveChunck").gameObject.SetActive(false);
            yield return new WaitForSecondsRealtime(0.2f);
            spRend.transform.Find("ActiveChunck").gameObject.SetActive(true);

            StartCoroutine(animateActiveChunck());

        }
        public void AnimatePoints(bool animate)
        {
            if (animate)
                StartCoroutine(AnimatePointsRoutine());
            else
            {
                StopAllCoroutines();
                foreach (var item in pointCollider)
                {
                    item.pointSprite.sprite = SpinWheel.Instance.pointSp[0];
                }
            }
        }
        IEnumerator AnimatePointsRoutine()
        {
            pointCollider[0].pointSprite.sprite = SpinWheel.Instance.pointSp[1];
            yield return new WaitForSeconds(0.1f);
            pointCollider[0].pointSprite.sprite = SpinWheel.Instance.pointSp[0];
            pointCollider[1].pointSprite.sprite = SpinWheel.Instance.pointSp[1];
            yield return new WaitForSeconds(0.1f);
            pointCollider[1].pointSprite.sprite = SpinWheel.Instance.pointSp[0];
            pointCollider[2].pointSprite.sprite = SpinWheel.Instance.pointSp[1];
            yield return new WaitForSeconds(0.1f);
            pointCollider[2].pointSprite.sprite = SpinWheel.Instance.pointSp[0];
            AnimatePoints(true);
        }
        public void AnimateChunkLoop(bool animate)
        {
            if (animate)
                StartCoroutine(AnimateChunkLoopRoutine());
            else
            {
                StopAllCoroutines();
                spRend.sprite = SpinWheel.Instance.chunkSp[0];
            }
        }
        IEnumerator AnimateChunkLoopRoutine()
        {
            spRend.sprite = SpinWheel.Instance.chunkSp[1];
            yield return new WaitForSeconds(0.1f);
            spRend.sprite = SpinWheel.Instance.chunkSp[0];
            yield return new WaitForSeconds(0.1f);
            AnimateChunkLoop(true);
        }
    }
}
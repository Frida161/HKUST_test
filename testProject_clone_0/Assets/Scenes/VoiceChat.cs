using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
using Agora.Rtc;
using TMPro;


public class VoiceChat : MonoBehaviour
{
    [FormerlySerializedAs("APP_ID")]
    [SerializeField]
    private string _appID = "";

    [FormerlySerializedAs("TOKEN")]
    [SerializeField]
    private string _token = "";

    [FormerlySerializedAs("CHANNEL_NAME")]
    [SerializeField]
    private string _channelName = "";

    internal IRtcEngine RtcEngine = null;
    private bool muteFlag = false;

    public Text LogText;
    public InputField ChannelInput;
    public Text JoinText;
    public Button TalkStateBtn;

    public Sprite muteSprite;
    public Sprite talkSprite;
    // Start is called before the first frame update
    void Start()
    {
        if (CheckAppId())
        {
            InitRtcEngine();
            SetBasicConfiguration();
        }
    }

    private bool CheckAppId()
    {
        if (_appID.Length > 10) return true;
        else
        {
            Debug.Log("Please fill in your appId!!!");
            return false;
        }
    }

    private void InitRtcEngine()
    {
        RtcEngine = Agora.Rtc.RtcEngine.CreateAgoraRtcEngine();
        UserEventHandler handler = new UserEventHandler(this);
        RtcEngineContext context = new RtcEngineContext(_appID, 0,
                                    CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_LIVE_BROADCASTING,
                                    AUDIO_SCENARIO_TYPE.AUDIO_SCENARIO_DEFAULT);
        RtcEngine.Initialize(context);
        RtcEngine.InitEventHandler(handler);
    }

    private void SetBasicConfiguration()
    {
        RtcEngine.EnableAudio();
        RtcEngine.SetChannelProfile(CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_COMMUNICATION);
        RtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
    }

    #region -- Button Events ---
    public void JoinChannel()
    {
        if (JoinText.text == "Join")
        {
            //默认声音频道
            if (ChannelInput.text == "") RtcEngine.JoinChannel(_token, _channelName);
            //指定声音频道
            else RtcEngine.JoinChannel(_token, ChannelInput.text);
        }
        else
        {
            RtcEngine.LeaveChannel();
        }
    }

    public void ChangeTalkState()
    {
        muteFlag = !muteFlag;
        RtcEngine.MuteLocalAudioStream(muteFlag);
        if (muteFlag) TalkStateBtn.image.sprite = muteSprite;
        else TalkStateBtn.image.sprite = talkSprite;
    }

    #endregion

    private void OnDestroy()
    {
        Debug.Log("OnDestroy");
        if (RtcEngine == null) return;
        RtcEngine.InitEventHandler(null);
        RtcEngine.LeaveChannel();
        RtcEngine.Dispose();
    }



}
#region -- Agora Event ---
internal class UserEventHandler : IRtcEngineEventHandler
{
    private readonly VoiceChat _audioSample;
    private Text LogText;
    private Text JoinText;

    internal UserEventHandler(VoiceChat audioSample)
    {
        _audioSample = audioSample;
        LogText = GameObject.Find("LogText").GetComponent<Text>();
        JoinText = GameObject.Find("JoinButton").transform.Find("Text").GetComponent<Text>();
    }

    public override void OnError(int err, string msg)
    {
        LogText.text =  (string.Format("OnError err: {0}, msg: {1}", err, msg));
    }

    public override void OnJoinChannelSuccess(RtcConnection connection, int elapsed)
    {
        LogText.text =  (
            string.Format("OnJoinChannelSuccess channelName: {0}, uid: {1}, elapsed: {2}",
                            connection.channelId, connection.localUid, elapsed));
        JoinText.text = "Leave";
    }

    public override void OnRejoinChannelSuccess(RtcConnection connection, int elapsed)
    {
        LogText.text =  ("OnRejoinChannelSuccess");
    }

    public override void OnLeaveChannel(RtcConnection connection, RtcStats stats)
    {
        LogText.text =  ("OnLeaveChannel");
        JoinText.text = "Join";
    }

    public override void OnClientRoleChanged(RtcConnection connection, CLIENT_ROLE_TYPE oldRole, CLIENT_ROLE_TYPE newRole, ClientRoleOptions newRoleOptions)
    {
        LogText.text =  ("OnClientRoleChanged");
    }

    public override void OnUserJoined(RtcConnection connection, uint uid, int elapsed)
    {
        LogText.text =  (string.Format("OnUserJoined uid: ${0} elapsed: ${1}", uid, elapsed));
    }

    public override void OnUserOffline(RtcConnection connection, uint uid, USER_OFFLINE_REASON_TYPE reason)
    {
        LogText.text =  (string.Format("OnUserOffLine uid: ${0}, reason: ${1}", uid,
            (int)reason));
    }
#endregion
}

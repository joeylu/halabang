using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Halabang.Audio {
  public class SoundListController : MonoBehaviour {
    public SoundController CurrentSong { get; private set; }
    public SoundController LastPlayedSong { get; private set; }
    public List<SoundController> SongList { get; private set; }
    public SoundPlayerController CurrentPlayer { get; set; }


    [SerializeField] private List<SoundController> defaultSongList;
    [SerializeField] private int defaultSongIndex;

    public int currentSongIndex; //当前播放的音乐 Index  //tony将其公开了
    private int totalSongCount; //计算当前歌曲列表总数
    public bool isPaused;//Tony新增变量

    private void Awake() {
      if (defaultSongList == null) Debug.LogError(name + " 必须指定至少一首歌");
      //初始化当前歌曲列表
      SongList = defaultSongList;
      foreach (SoundController sc in SongList) {
        sc.CurrentSoundListController = this;
      }
      //默认列表的第一首歌为当前播放的歌，或者为给与的排序歌曲
      CurrentSong = (defaultSongIndex == 0) ? SongList.First() : SongList[defaultSongIndex];
      currentSongIndex = defaultSongIndex;
      totalSongCount = SongList.Count;
    }

    public void Play() {
      CurrentSong.Play();
      isPaused = false;//tony新增bool状态变更
    }
    public void Stop() {
      CurrentSong.Stop();
    }
    public void Pause() {
      CurrentSong.Pause();
      isPaused = true; //tony新增bool状态变更
    }
    public void Prev() {
      Stop();
      if (currentSongIndex == 0) {
        //如果当前播放的是第一首歌，回到最后一首歌
        currentSongIndex = totalSongCount - 1;
      } else {
        currentSongIndex -= 1;
      }
      CurrentSong = SongList[currentSongIndex];
      Play();
    }
    public void Next() {
      Stop();
      if (currentSongIndex == totalSongCount -1) {
        //如果当前播放的是最后一首歌，回到第一首歌
        currentSongIndex = 0;
      } else {
        currentSongIndex += 1;
      }
      CurrentSong = SongList[currentSongIndex];
      Play();
    }

    //tony添加的公开函数
    public void Select(int index) {
    Stop();
    CurrentSong = SongList[index];
    currentSongIndex = index;
    Play();
    }

    public void ChangeVolume() {
      CurrentSong.ChangeVolume();
    }
  }
}
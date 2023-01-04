using System;
using UnityEngine;

namespace BoundfoxStudios.MiniGolf._Game.Scripts
{
    public class TrackManager : MonoBehaviour
    {
        public Track[] Tracks;
        public Player Player;

        private int _currentTrack;

        private void Start()
        {
            if (Player && Tracks.Length > 0)
            {
                _currentTrack = 0;
                Player.SpawnTo(Tracks[_currentTrack].SpawnPoint.position);
            }
        }

        public void NextTrack()
        {
            // Before going to next track, reset flag
            Tracks[_currentTrack].flag.resetFlagPosition();
            
            // Calculate next track
            _currentTrack = (_currentTrack + 1) % Tracks.Length;

            // Spawn player to next flag
            Player.SpawnTo(Tracks[_currentTrack].SpawnPoint.position);

        }

        public Vector3 getFlagPosition() { return Tracks[_currentTrack].flag.getPosition(); ; }

        public void elevateFlagTo(float coordY) 
        {
            Vector3 position = getFlagPosition();
            position.y = coordY;
            Tracks[_currentTrack].flag.moveTo(position);
        }
    }
}

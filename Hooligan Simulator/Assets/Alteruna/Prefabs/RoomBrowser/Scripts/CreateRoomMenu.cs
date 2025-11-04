using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Alteruna
{
    public class CreateRoomMenu : BaseRoomBrowser
    {
        [Range(0, 30)]
        public int MaxNameLength = 20;

        [Range(2, 10)]
        public int MaxPlayers = 10;

        [SerializeField] private TMP_InputField _inputRoomName;
        [SerializeField] private TMP_InputField _inputMaxPlayers;
        [SerializeField] private TMP_InputField _inputPassword;
        [SerializeField] private Toggle _toggleHideRoom;
        [SerializeField] private Button _buttonCreateRoom;

        private CustomRoomInfo _customRoomInfo;

        private new void OnEnable()
        {
            base.OnEnable();
            Multiplayer.OnRoomCreated.AddListener(CreatedRoom);
        }

        private void OnDisable()
        {
            Multiplayer.OnRoomCreated.RemoveListener(CreatedRoom);
        }

        void Start()
        {
            _customRoomInfo = new CustomRoomInfo();

            RoomNameChanged(Multiplayer.Me.Name);

            _inputRoomName.characterLimit = MaxNameLength;

            _inputRoomName.onValueChanged.AddListener(RoomNameChanged);
            _inputMaxPlayers.onEndEdit.AddListener(MaxPlayersChanged);

            _toggleHideRoom.onValueChanged.AddListener(ToggleHideRoom);

            _buttonCreateRoom.onClick.AddListener(Submit);
        }

        private void ToggleHideRoom(bool value)
        {
            _inputPassword.transform.parent.gameObject.SetActive(!value);
        }

        public void ChangeMaxPlayersValue(int value)
        {
            int maxPlayers = int.Parse(_inputMaxPlayers.text) + value;
            maxPlayers = HandleMaxPlayers(maxPlayers);
            _inputMaxPlayers.SetTextWithoutNotify(maxPlayers.ToString());
        }

        #region Callbacks

        private void RoomNameChanged(string value)
        {
            HandleRoomName(value);

            if (_customRoomInfo.RoomName != value)
                _inputRoomName.SetTextWithoutNotify(_customRoomInfo.RoomName);
        }

        private void MaxPlayersChanged(string value)
        {
            if (!int.TryParse(value, out int maxPlayers))
            {
                maxPlayers = MaxPlayers;
            }
            else
            {
                maxPlayers = HandleMaxPlayers(maxPlayers);
            }

            _inputMaxPlayers.SetTextWithoutNotify(maxPlayers.ToString());
        }

        private void CreatedRoom(Multiplayer multiplayer, bool success, Room room, string inviteCode)
        {
            gameObject.SetActive(false);

            if (success)
            {
                Debug.Log("Room created successfully.");
            }
            else
            {
                Debug.LogError("Failed to create room!");
            }
        }

        #endregion

        private void HandleRoomName(string value)
        {
            _customRoomInfo.RoomName = value.Length > MaxNameLength ? value.Substring(0, MaxNameLength) : value;
        }

        private int HandleMaxPlayers(int value)
        {
            if (value < 2)
                value = 2;
            else if (value > MaxPlayers)
                value = MaxPlayers;

            return value;
        }

        public void Submit()
        {
            if (Multiplayer.InRoom)
            {
                StatusPopup.Instance.TriggerStatus("Invalid action!\nAlready in a room!");
                return;
            }

            bool maxPlayersValid = int.TryParse(_inputMaxPlayers.text, out int maxPlayers);
            ushort password = FormatPassword(_inputPassword.text);

            if (_toggleHideRoom.isOn)
                password = (ushort)UnityEngine.Random.Range(1, 1024);

            HandleRoomName(_inputRoomName.text);

            _customRoomInfo.RoomName = _customRoomInfo.RoomName.Trim();

            if (_customRoomInfo.RoomName.Length > MaxNameLength)
            {
                StatusPopup.Instance.TriggerStatus($"Invalid value!\n[Room Name] can be no longer than '{MaxNameLength}' characters!");
                return;
            }

            if (!maxPlayersValid || maxPlayers > MaxPlayers || maxPlayers < 2)
            {
                StatusPopup.Instance.TriggerStatus($"Invalid value!\n[Max Players] need to be between 2 and {MaxPlayers}!");
                return;
            }

            string roomInfo = Writer.SerializeAndPackString(_customRoomInfo);

            if (!_toggleHideRoom.isOn)
                Multiplayer.CreateRoom(roomInfo, _toggleHideRoom.isOn, password, true, true, (ushort)maxPlayers);
            else
                Multiplayer.CreatePrivateRoom(_customRoomInfo.RoomName, (ushort)MaxPlayers, true, true);
        }

        public static ushort FormatPassword(string password)
        {
            if (password.Length == 0) return 0;

            if (int.TryParse(password, out int pin))
            {
                if (pin < 0) return (ushort)-pin;
                if (pin != 0 && (ushort)pin == 0) return (ushort)(pin >> 8);
                return (ushort)pin;
            }

            ushort hash = 0;
            foreach (char c in password)
            {
                hash += c;
            }

            if (hash == 0) return ushort.MaxValue;
            return hash;
        }

#if UNITY_EDITOR
        private new void Reset()
        {
            base.Reset();
        }
#endif
    }
}

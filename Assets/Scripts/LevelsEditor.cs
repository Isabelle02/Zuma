#if UNITY_EDITOR

using System.Collections.Generic;
using System.Linq;
using BansheeGz.BGSpline.Curve;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class LevelsEditor : EditorWindow
{
    private ReorderableList _levels;
    private ReorderableList _curves;
    private ReorderableList _ballSprites;

    private SerializedObject _so;

    private Dictionary<BGCurve, CurveData> _curveUnion = new Dictionary<BGCurve, CurveData>();

    private PlayerView _player;

    [MenuItem("Window/LevelsEditor")]
    private static void Init() => GetWindow<LevelsEditor>("LevelsEditorWindow", true);
    
    private void OnHierarchyChange()
    {
        if (_levels?.index > -1)
        {
            LevelManager.LevelsConfig.Levels[_levels.index].Curves.Clear();
            var curveObjs = _curveUnion.Keys.ToList();
            foreach (var k in curveObjs)
            {
                var cd = new CurveData();
                cd.Duration = _curveUnion[k].Duration;
                foreach (var p in k.Points)
                {
                    var pointData = new PointData
                    {
                        Position = p.PositionWorld,
                        ControlFirst = p.ControlFirstWorld,
                        ControlSecond = p.ControlSecondWorld,
                        ControlType = p.ControlType
                    };
                    cd.Points.Add(pointData);
                }

                _curveUnion[k] = cd;
                LevelManager.LevelsConfig.Levels[_levels.index].Curves.Add(cd);
            }
        }
        
        AssetDatabase.SaveAssets();
    }

    private void OnEnable()
    {
        LevelManager.LevelsConfig.Levels = LevelManager.LoadLevels();
        
        _levels = new ReorderableList(LevelManager.LevelsConfig.Levels, typeof(LevelData),
            true, true, true, true)
        {
            drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                rect.y += 2;
                EditorGUI.LabelField(rect, "Level " + (index + 1));
            },
            drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "Levels");
            },
            onAddCallback = list =>
            {
                LevelManager.LevelsConfig.Levels.Add(new LevelData());
            },
            onRemoveCallback = list =>
            {
                LevelManager.LevelsConfig.Levels.RemoveAt(list.index);
            },
            onSelectCallback = list =>
            {
                var players = FindObjectsOfType<PlayerView>().ToList();
                for (var i = 0; i < players.Count; i++)
                {
                    DestroyImmediate(players[i].gameObject);
                    players.RemoveAt(i);
                    i--;
                }

                LevelManager.LevelsConfig.Levels[list.index].Player ??= new PlayerData();
                _player = Instantiate(LevelManager.LevelsConfig.PlayerPrefab);
                
                var curves = FindObjectsOfType<BGCurve>().ToList();
                for (var i = 0; i < curves.Count; i++)
                {
                    DestroyImmediate(curves[i].gameObject);
                    curves.RemoveAt(i);
                    i--;
                }

                _curveUnion.Clear();
                foreach (var c in LevelManager.LevelsConfig.Levels[list.index].Curves)
                {
                    var curveObj = Instantiate(LevelManager.LevelsConfig.BgCurvePrefab);
                    foreach (var p in c.Points)
                        curveObj.AddPoint(curveObj.CreatePointFromWorldPosition(
                            p.Position, p.ControlType, p.ControlFirst, p.ControlSecond));

                    _curveUnion.Add(curveObj, c);
                }
                OnHierarchyChange();
                
                _curves = new ReorderableList(LevelManager.LevelsConfig.Levels[list.index].Curves, typeof(BGCurve),
                    false, true, true, true)
                {
                    drawElementCallback = (rect, index, isActive, isFocused) =>
                    {
                        rect.y += 2;

                        LevelManager.LevelsConfig.Levels[_levels.index].Curves[index].Duration = EditorGUI.FloatField(
                            rect, "Curve " + index + "\tDuration", LevelManager.LevelsConfig.Levels[_levels.index].Curves[index].Duration);
                    },
                    drawHeaderCallback = rect =>
                    {
                        EditorGUI.LabelField(rect, "Curves");
                    },
                    onAddCallback = cList =>
                    {
                        var curveObj = Instantiate(LevelManager.LevelsConfig.BgCurvePrefab);
                        var curveData = new CurveData();
                        foreach (var p in curveObj.Points)
                        {
                            var pointData = new PointData
                            {
                                Position = p.PositionWorld,
                                ControlFirst = p.ControlFirstWorld,
                                ControlSecond = p.ControlSecondWorld,
                                ControlType = p.ControlType
                            };
                            curveData.Points.Add(pointData);
                        }
                        
                        _curveUnion.Add(curveObj, curveData);
                        LevelManager.LevelsConfig.Levels[list.index].Curves.Add(curveData);
                    },
                    onRemoveCallback = cList =>
                    {
                        var key = _curveUnion.FirstOrDefault(c =>
                            c.Value == LevelManager.LevelsConfig.Levels[list.index].Curves[cList.index]).Key;
                        if (key != null)
                        {
                            _curveUnion.Remove(key);
                            DestroyImmediate(key.gameObject);
                        }

                        LevelManager.LevelsConfig.Levels[list.index].Curves.RemoveAt(cList.index);
                    }
                };
            }
        };
    }

    private void OnGUI()
    {
        GUILayout.Label("Level Settings", EditorStyles.boldLabel);
        _levels?.DoLayoutList();
        
        using (new GUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Load"))
                if (_levels != null && _levels.IsSelected(_levels.index))
                {
                    LevelManager.LevelsConfig.Levels[_levels.index] = LevelManager.LoadLevel(_levels.index);
                }

            if (GUILayout.Button("Save"))
            {
                AssetDatabase.SaveAssets();
                LevelManager.SaveLevels();
            }
        }

        _curves?.DoLayoutList();

        if (_levels != null && _levels.IsSelected(_levels.index))
        {
            LevelManager.LevelsConfig.Levels[_levels.index].BallColorCount = EditorGUILayout.IntField(
                "BallColorCount", LevelManager.LevelsConfig.Levels[_levels.index].BallColorCount);
            foreach (var c in LevelManager.LevelsConfig.Levels[_levels.index].Curves)
                c.ColorCount = LevelManager.LevelsConfig.Levels[_levels.index].BallColorCount;
            
            LevelManager.LevelsConfig.Levels[_levels.index].StartBoardPosition = EditorGUILayout.Vector3Field(
                "StartBoardPosition", LevelManager.LevelsConfig.Levels[_levels.index].StartBoardPosition);
            foreach (var c in LevelManager.LevelsConfig.Levels[_levels.index].Curves)
                c.StartBoardPosition = LevelManager.LevelsConfig.Levels[_levels.index].StartBoardPosition;
            
            LevelManager.LevelsConfig.Levels[_levels.index].LimitBallCount = EditorGUILayout.IntField(
                "LimitBallCount", LevelManager.LevelsConfig.Levels[_levels.index].LimitBallCount);
            foreach (var c in LevelManager.LevelsConfig.Levels[_levels.index].Curves)
                c.LimitBallCount = LevelManager.LevelsConfig.Levels[_levels.index].LimitBallCount;

            LevelManager.LevelsConfig.Levels[_levels.index].Player.Position = EditorGUILayout.Vector3Field(
                "PlayerPosition", LevelManager.LevelsConfig.Levels[_levels.index].Player.Position);
            if (_player != null)
                _player.transform.position = LevelManager.LevelsConfig.Levels[_levels.index].Player.Position;
        }
    }
}

#endif
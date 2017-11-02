﻿using UnityEngine;
using System.Collections;

namespace ASL.FogOfWar
{
    internal class FOWMaskTexture
    {

        public Texture2D texture { get { return m_MaskTexture; } }

        private Texture2D m_MaskTexture;

        private byte[,] m_MaskCache;
        private byte[,] m_Visible;

        private bool m_IsUpdated;

        private int m_Width;
        private int m_Height;

        public FOWMaskTexture(int width, int height)
        {
            m_Width = width;
            m_Height = height;
            m_MaskCache = new byte[width, height];
            m_Visible = new byte[width, height];
        }

        public void SetAsVisible(int x, int y)
        {
            m_MaskCache[x, y] = 1;
            m_IsUpdated = true;
        }

        public bool IsVisible(int x, int y)
        {
            if (x < 0 || x >= m_Width || y < 0 || y >= m_Height)
                return false;
            return m_Visible[x, y] == 1;
        }

        public bool RefreshTexture()
        {
            if (!m_IsUpdated)
                return false;
            bool isNew = false;
            if (m_MaskTexture == null)
            {
                m_MaskTexture = new Texture2D(m_Width, m_Height);
                m_MaskTexture.wrapMode = TextureWrapMode.Clamp;
                isNew = true;
            }
            for (int i = 0; i < m_MaskTexture.width; i++)
            {
                for (int j = 0; j < m_MaskTexture.height; j++)
                {
                    bool isVisible = m_MaskCache[i, j] == 1;
                    Color origin = isNew ? Color.black : m_MaskTexture.GetPixel(i, j);
                    origin.r = Mathf.Clamp01(origin.r + origin.g);
                    origin.b = origin.g;
                    origin.g = isVisible ? 1 : 0;
                    m_MaskTexture.SetPixel(i, j, origin);
                    m_Visible[i, j] = (byte)(isVisible ? 1 : 0);
                    m_MaskCache[i, j] = 0;
                }
            }
            m_MaskTexture.Apply();
            m_IsUpdated = false;
            return true;
        }

        public void Release()
        {
            if (m_MaskTexture != null)
                Object.Destroy(m_MaskTexture);
            m_MaskTexture = null;
            m_MaskCache = null;
            m_Visible = null;
        }
    }
}
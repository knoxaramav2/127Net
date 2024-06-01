package com.knx.homemobile.ui.component

import android.content.Context
import android.graphics.BlendMode
import android.graphics.BlendModeColorFilter
import android.graphics.Color
import android.os.Build
import android.util.AttributeSet
import android.widget.Button
import android.widget.LinearLayout
import androidx.annotation.RequiresApi
import androidx.core.content.ContextCompat
import com.knx.homemobile.R

@RequiresApi(Build.VERSION_CODES.Q)
class device_select(ctx: Context?, attrs: AttributeSet? = null) : LinearLayout(ctx, attrs) {
    private val deviceInfoBtn: Button
    private val deviceDelBtn: Button

    init{
        orientation = HORIZONTAL
        deviceInfoBtn = Button(ctx).apply {
            text = "Device"
            layoutParams = LayoutParams(LayoutParams.WRAP_CONTENT, LayoutParams.WRAP_CONTENT, 1f)
        }

        deviceDelBtn = Button(ctx).apply {
            text = resources.getString(R.string.btn_delete)
            layoutParams = LayoutParams(0, LayoutParams.WRAP_CONTENT, 1f)
        }

        deviceDelBtn.background.colorFilter = BlendModeColorFilter(Color.RED, BlendMode.MULTIPLY)
        deviceDelBtn.setTextColor(Color.WHITE)

        addView(deviceInfoBtn)
        addView(deviceDelBtn)
    }
}